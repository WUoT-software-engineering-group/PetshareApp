using Petshare.CrossCutting.DTO.Pet;

namespace Petshare.Services.Abstract
{
    public interface IPetService
    {
        Task<PetResponse?> Create(Guid shelterId, PostPetRequest pet);
        Task<PetResponse?> GetById(Guid petId);
        Task<List<PetResponse>> GetByShelter(Guid shelterId);
        Task<bool> Update(Guid petId, PutPetRequest pet);
    }
}