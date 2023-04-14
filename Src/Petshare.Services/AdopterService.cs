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
        // TODO: Ustawić domyślny status, jak już będzie w bazie

        var createdAdopter = await _repositoryWrapper.AdopterRepository.Create(adopterToCreate);
        await _repositoryWrapper.Save();

        return createdAdopter.ID;
    }

    public async Task<List<GetAdopterResponse>> GetAll()
    {
        var adopters = await _repositoryWrapper.AdopterRepository.FindAll();

        return adopters.Adapt<List<GetAdopterResponse>>();
    }

    public async Task<GetAdopterResponse?> GetById(Guid id)
    {
        var adopter = (await _repositoryWrapper.AdopterRepository.FindByCondition(a => a.ID == id)).SingleOrDefault();
        return adopter?.Adapt<GetAdopterResponse>();
    }
}