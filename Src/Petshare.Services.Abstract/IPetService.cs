using Petshare.CrossCutting.DTO.Pet;

namespace Petshare.Services.Abstract
{
    public interface IPetService
    {
        Task<PetResponse?> Create(Guid shelterId, PostPetRequest pet);

        Task<bool> Update(Guid petId, PutPetRequest pet);

        Task<PetResponse?> GetById(Guid petId);

        // TODO: uncomment when auth is added
        Task<List<PetResponse>> GetByShelter(/*Guid shelterId*/);
    }
}