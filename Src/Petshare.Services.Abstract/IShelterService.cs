

using Petshare.CrossCutting.DTO;

namespace Petshare.Services.Abstract
{
    public interface IShelterService
    {
        Task<List<ShelterDTO>> GetAll();

        Task<ShelterDTO?> GetById(string id);

        Task<ShelterDTO> Create(ShelterDTO shelter);

        Task<bool> Update(string id, ShelterDTO shelter);
    }
}