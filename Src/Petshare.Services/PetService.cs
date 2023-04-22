using System.Net.Http.Json;
using Mapster;
using Microsoft.AspNetCore.Http;
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

        public async Task<Guid?> Create(Guid shelterId, PostPetRequest pet)
        {
            var petToCreate = pet.Adapt<Pet>();

            var petShelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(x => x.ID == shelterId)).SingleOrDefault();
            if (petShelter is null)
            {
                return null;
            }

            petToCreate.Shelter = petShelter;

            var createdPet = await _repositoryWrapper.PetRepository.Create(petToCreate);
            await _repositoryWrapper.Save();

            return createdPet.ID;
        }

        public async Task<bool> Update(Guid userId, string? role, Guid petId, PutPetRequest pet)
        {
            var petToUpdate = (await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId)).SingleOrDefault();
            if (petToUpdate is null
                || (role == "shelter" && petToUpdate.Shelter.ID != userId))
            {
                return false;
            }

            await _repositoryWrapper.PetRepository.Update(pet.Adapt(petToUpdate));
            await _repositoryWrapper.Save();

            return true;
        }

        public async Task<bool> UpdatePhotoUri(Guid petId, Guid shelterId, string photoUri)
        {
            var petToUpdate = (await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId)).SingleOrDefault();
            if (petToUpdate is null || petToUpdate.Shelter.ID != shelterId)
            {
                return false;
            }

            petToUpdate.PhotoUri = photoUri;
            await _repositoryWrapper.PetRepository.Update(petToUpdate);
            await _repositoryWrapper.Save();

            return true;
        }

        public async Task<PetResponse?> GetById(Guid petId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository.FindByCondition(x => x.ID == petId);
            return petsByShelter.SingleOrDefault()?.Adapt<PetResponse>();
        }

        public async Task<List<PetResponse>> GetByShelter(Guid shelterId)
        {
            var petsByShelter = await _repositoryWrapper.PetRepository
                .FindByCondition(x => x.Shelter.ID == shelterId);
            return petsByShelter.ToList().Adapt<List<PetResponse>>();
        }
    }
}
