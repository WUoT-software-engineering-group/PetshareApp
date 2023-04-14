using Petshare.CrossCutting.DTO.Adopter;

namespace Petshare.Services.Abstract;

public interface IAdopterService
{
    Task<Guid> Create(PostAdopterRequest adopterRequest);
}