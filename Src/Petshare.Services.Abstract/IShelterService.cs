using Petshare.CrossCutting.DTO;

namespace Petshare.Services.Abstract
{
    public interface IShelterService
    {
        Task<List<ShelterDTO>> GetAll();

        Task<ShelterDTO?> GetById(Guid id);

        Task<ShelterDTO> Create(ShelterDTO shelter);

        Task<bool> Update(ShelterDTO shelter);
    }
}