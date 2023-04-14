using Mapster;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class AdopterService : IAdopterService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public AdopterService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<Guid> Create(PostAdopterRequest adopterRequest)
    {
        var adopterToCreate = adopterRequest.Adapt<Adopter>();

        var createdAdopter = await _repositoryWrapper.AdopterRepository.Create(adopterToCreate);
        await _repositoryWrapper.Save();

        return createdAdopter.ID;
    }
}