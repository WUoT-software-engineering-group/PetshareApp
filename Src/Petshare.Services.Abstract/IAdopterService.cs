using Petshare.CrossCutting.DTO.Adopter;

namespace Petshare.Services.Abstract;

public interface IAdopterService
{
    Task<List<GetAdopterResponse>> GetAll();

    Task<GetAdopterResponse?> GetById(Guid id);

    Task<Guid> Create(PostAdopterRequest adopterRequest);
}