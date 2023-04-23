using Petshare.CrossCutting.DTO.Adopter;

namespace Petshare.Services.Abstract;

public interface IAdopterService
{
    Task<List<GetAdopterResponse>> GetAll();

    Task<GetAdopterResponse?> GetById(Guid id);

    Task<Guid> Create(PostAdopterRequest adopterRequest);

    Task<bool> UpdateStatus(Guid id, PutAdopterRequest adopter);

    Task VerifyAdopterForShelter(Guid adopterId, Guid shelterId);

    Task<bool> CheckIfAdopterIsVerified(Guid adopterId, Guid shelterId);
}