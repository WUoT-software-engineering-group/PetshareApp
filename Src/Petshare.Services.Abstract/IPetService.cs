using Petshare.CrossCutting.DTO;

namespace Petshare.Services.Abstract
{
    public interface IPetService
    {
        Task<PetDTO?> Create(PetDTO pet);
        Task<PetDTO?> GetById(Guid petId);
        Task<List<PetDTO>> GetByShelter(Guid shelterId);
        Task<bool> Update(PetDTO pet);
    }
}