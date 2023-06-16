using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Utils;

namespace Petshare.Services.Abstract
{
    public interface IShelterService
    {
        Task<ServiceResponse> Create(PostShelterRequest shelter);

        Task<ServiceResponse> Update(Guid id, PutShelterRequest shelter);

        Task<ServiceResponse> GetAll(PagingRequest pagingRequest);

        Task<ServiceResponse> GetById(Guid id);
    }
}