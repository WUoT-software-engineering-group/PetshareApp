using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.Utils;

namespace Petshare.Services.Abstract
{
    public interface IPetService
    {
        Task<ServiceResponse> Create(Guid shelterId, PostPetRequest pet);

        Task<ServiceResponse> Update(Guid userId, string? role, Guid petId, PutPetRequest pet);

        Task<ServiceResponse> UpdatePhotoUri(Guid petId, Guid shelterId, string photoUri);

        Task<ServiceResponse> GetById(Guid petId);

        Task<ServiceResponse> GetByShelter(Guid shelterId);
    }
}