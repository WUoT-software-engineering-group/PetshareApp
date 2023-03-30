using Petshare.CrossCutting.DTO.Shelter;

namespace Petshare.Services.Abstract
{
    public interface IShelterService
    {
        Task<ShelterResponse> Create(PostShelterRequest shelter);

        Task<bool> Update(Guid id, PutShelterRequest shelter);

        Task<List<ShelterResponse>> GetAll();

        Task<ShelterResponse?> GetById(Guid id);
    }
}