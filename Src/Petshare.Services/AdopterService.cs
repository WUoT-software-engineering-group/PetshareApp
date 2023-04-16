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

    public async Task<Guid> Create(PostAdopterRequest adopterRequest)
    {
        var adopterToCreate = adopterRequest.Adapt<Adopter>();

        var createdAdopter = await _repositoryWrapper.AdopterRepository.Create(adopterToCreate);
        await _repositoryWrapper.Save();

        return createdAdopter.ID;
    }

    public async Task<bool> UpdateStatus(Guid id, PutAdopterRequest adopter)
    {
        var adopterToUpdate = (await _repositoryWrapper.AdopterRepository.FindByCondition(a => a.ID == id))
            .SingleOrDefault();

        if (adopterToUpdate is null)
            return false;

        adopterToUpdate = adopter.Adapt(adopterToUpdate);
        await _repositoryWrapper.AdopterRepository.Update(adopterToUpdate);
        await _repositoryWrapper.Save();

        return true;
    }

    public async Task VerifyAdopterForShelter(Guid adopterId, Guid shelterId)
    {
        var verification = new ShelterAdopterVerification
        {
            AdopterID = adopterId,
            ShelterID = shelterId
        };

        var createdVerification = await _repositoryWrapper.ShelterAdopterVerificationRepository.Create(verification);
        await _repositoryWrapper.Save();
    }

    public async Task<bool> CheckIfAdopterIsVerified(Guid adopterId, Guid shelterId)
    {
        var verification = (await _repositoryWrapper.ShelterAdopterVerificationRepository.FindByCondition(v =>
                       v.AdopterID == adopterId && v.ShelterID == shelterId)).SingleOrDefault();

        return verification is not null;
    }
}