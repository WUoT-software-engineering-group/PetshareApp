using Mapster;
using Petshare.CrossCutting.DTO.Pet;
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

        public async Task<PetResponse?> Create(Guid shelterId, PostPetRequest pet)
        {
            var petToCreate = pet.Adapt<Pet>();
            petToCreate.ID = Guid.NewGuid();

            var petShelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(x => x.ID == shelterId)).SingleOrDefault();
            if (petShelter is null)
            {
                return null;
            }

            petToCreate.Shelter = petShelter;

            var createdPet = await _repositoryWrapper.PetRepository.Create(petToCreate);
            await _repositoryWrapper.Save();

            return createdPet?.Adapt<PetResponse>();
        }

        public async Task<PetResponse?> GetById(Guid petId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId);
            return petsByShelter.SingleOrDefault()?.Adapt<PetResponse>();
        }

        public async Task<List<PetResponse>> GetByShelter(Guid shelterId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository.FindByCondition(x => x.Shelter.ID == shelterId);
            return petsByShelter.ToList().Adapt<List<PetResponse>>();
        }

        public async Task<bool> Update(Guid petId, PutPetRequest pet)
        {
            var petToUpdate = (await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId)).SingleOrDefault();
            if (petToUpdate is null)
            {
                return false;
            }

            await _repositoryWrapper.PetRepository.Update(pet.Adapt(petToUpdate));
            await _repositoryWrapper.Save();

            return true;
        }
    }
}
