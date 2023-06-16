using Mapster;
using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.Utils;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services
{
    public class PetService : IPetService
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public PetService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<ServiceResponse> Create(Guid shelterId, PostPetRequest pet)
        {
            var petToCreate = pet.Adapt<Pet>();

            var petShelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(x => x.ID == shelterId)).SingleOrDefault();
            if (petShelter is null)
            {
                return ServiceResponse.BadRequest();
            }

            petToCreate.Shelter = petShelter;

            var createdPet = await _repositoryWrapper.PetRepository.Create(petToCreate);
            await _repositoryWrapper.Save();

            return ServiceResponse.Created(createdPet.ID);
        }

        public async Task<ServiceResponse> Update(Guid userId, string? role, Guid petId, PutPetRequest pet)
        {
            var petToUpdate = (await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId)).SingleOrDefault();
            if (petToUpdate is null
                || (role == "shelter" && petToUpdate.Shelter.ID != userId))
            {
                return ServiceResponse.BadRequest();
            }

            await _repositoryWrapper.PetRepository.Update(pet.Adapt(petToUpdate));
            await _repositoryWrapper.Save();

            return ServiceResponse.Ok();
        }

        public async Task<ServiceResponse> UpdatePhotoUri(Guid petId, Guid shelterId, string photoUri)
        {
            var petToUpdate = (await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId)).SingleOrDefault();
            if (petToUpdate is null || petToUpdate.Shelter.ID != shelterId)
            {
                return ServiceResponse.BadRequest();
            }

            petToUpdate.PhotoUri = photoUri;
            await _repositoryWrapper.PetRepository.Update(petToUpdate);
            await _repositoryWrapper.Save();

            return ServiceResponse.Ok();
        }

        public async Task<ServiceResponse> GetById(Guid petId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId);
            var pet = petsByShelter.SingleOrDefault();

            return pet is not null
                ? ServiceResponse.Ok(pet.Adapt<PetResponse>())
                : ServiceResponse.NotFound();
        }

        public async Task<ServiceResponse> GetByShelter(Guid shelterId, PagingRequest pagingRequest)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository
                .FindByCondition(x => x.Shelter.ID == shelterId);

            return ServiceResponse.Ok(new PagedPetResponse
            {
                Pets = petsByShelter.Skip(pagingRequest.PageNumber * pagingRequest.PageCount).Take(pagingRequest.PageCount).ToList().Adapt<List<PetResponse>>(),
                PageNumber = pagingRequest.PageNumber,
                Count = petsByShelter.Count()
            });
        }
    }
}
