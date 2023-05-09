using Mapster;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.Utils;
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

    public async Task<ServiceResponse> GetAll()
    {
        var adopters = await _repositoryWrapper.AdopterRepository.FindAll();

        return ServiceResponse.Ok(adopters.Adapt<List<GetAdopterResponse>>());
    }

    public async Task<ServiceResponse> GetById(Guid id)
    {
        var adopter = (await _repositoryWrapper.AdopterRepository.FindByCondition(a => a.ID == id)).SingleOrDefault();
        return adopter is not null 
            ? ServiceResponse.Ok(adopter.Adapt<GetAdopterResponse>())
            : ServiceResponse.NotFound();
    }

    public async Task<ServiceResponse> Create(PostAdopterRequest adopterRequest)
    {
        var adopterToCreate = adopterRequest.Adapt<Adopter>();

        var createdAdopter = await _repositoryWrapper.AdopterRepository.Create(adopterToCreate);
        await _repositoryWrapper.Save();

        return ServiceResponse.Created(createdAdopter.ID);
    }

    public async Task<ServiceResponse> UpdateStatus(Guid id, PutAdopterRequest adopter)
    {
        var adopterToUpdate = (await _repositoryWrapper.AdopterRepository.FindByCondition(a => a.ID == id))
            .SingleOrDefault();

        if (adopterToUpdate is null)
            return ServiceResponse.NotFound();

        adopterToUpdate = adopter.Adapt(adopterToUpdate);
        await _repositoryWrapper.AdopterRepository.Update(adopterToUpdate);
        await _repositoryWrapper.Save();

        return ServiceResponse.Ok();
    }

    public async Task VerifyAdopterForShelter(Guid adopterId, Guid shelterId)
    {
        var verification = new ShelterAdopterVerification
        {
            AdopterID = adopterId,
            ShelterID = shelterId
        };

        await _repositoryWrapper.ShelterAdopterVerificationRepository.Create(verification);
        await _repositoryWrapper.Save();
    }

    public async Task<ServiceResponse> CheckIfAdopterIsVerified(Guid adopterId, Guid shelterId)
    {
        var verification = (await _repositoryWrapper.ShelterAdopterVerificationRepository.FindByCondition(v =>
                       v.AdopterID == adopterId && v.ShelterID == shelterId)).SingleOrDefault();

        return ServiceResponse.Ok(verification is not null);
    }
}