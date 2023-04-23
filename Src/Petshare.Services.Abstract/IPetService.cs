using Microsoft.AspNetCore.Http;
using Petshare.CrossCutting.DTO.Pet;

namespace Petshare.Services.Abstract
{
    public interface IPetService
    {
        Task<Guid?> Create(Guid shelterId, PostPetRequest pet);

        Task<bool> Update(Guid userId, string? role, Guid petId, PutPetRequest pet);

        Task<bool> UpdatePhotoUri(Guid petId, Guid shelterId, string photoUri);

        Task<PetResponse?> GetById(Guid petId);

        Task<List<PetResponse>> GetByShelter(Guid shelterId);
    }
}