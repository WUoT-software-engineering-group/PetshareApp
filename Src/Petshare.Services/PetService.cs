using Mapster;
using Petshare.CrossCutting.DTO;
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

        public async Task<PetDTO?> GetById(Guid petId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId);
            return petsByShelter.SingleOrDefault()?.Adapt<PetDTO>();
        }

        public async Task<List<PetDTO>> GetByShelter(Guid shelterId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository.FindByCondition(x => x.Shelter.ID == shelterId);
            return petsByShelter.ToList().Adapt<List<PetDTO>>();
        }

        public async Task<PetDTO?> Create(PetDTO pet)
        {
            var petToCreate = pet.Adapt<Pet>();
            petToCreate.ID = Guid.NewGuid();

            var petShelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(x => x.ID == pet.ShelterID)).SingleOrDefault();
            if (petShelter is null)
            {
                return null;
            }

            petToCreate.Shelter = petShelter;

            var createdPet = await _repositoryWrapper.PetRepository.Create(petToCreate);
            await _repositoryWrapper.Save();

            return createdPet?.Adapt<PetDTO>();
        }

        public async Task<bool> Update(PetDTO pet)
        {
            var petToUpdate = (await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == pet.ID)).SingleOrDefault();
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
