﻿using Mapster;
using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.DTO.Shelter;
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

    public async Task<ServiceResponse> GetAll(PagingRequest pagingRequest)
    {
        var adopters = await _repositoryWrapper.AdopterRepository.FindAll();

        return ServiceResponse.Ok(new PagedAdopterResponse
        {
            Adopters = adopters.Skip(pagingRequest.PageNumber * pagingRequest.PageCount).Take(pagingRequest.PageCount).Adapt<List<GetAdopterResponse>>(),
            PageNumber = pagingRequest.PageNumber,
            Count = adopters.Count()
        });
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

    public async Task<ServiceResponse> VerifyAdopterForShelter(Guid adopterId, Guid shelterId)
    {
        var adopter = (await _repositoryWrapper.AdopterRepository.FindByCondition(a => a.ID == adopterId))
            .SingleOrDefault();
        if (adopter is null)
            return ServiceResponse.NotFound();

        var verification = (await _repositoryWrapper.ShelterAdopterVerificationRepository.FindByCondition(v =>
            v.AdopterID == adopterId && v.ShelterID == shelterId)).SingleOrDefault();
        if (verification is not null)
            return ServiceResponse.BadRequest();

        var newVerification = new ShelterAdopterVerification
        {
            AdopterID = adopterId,
            ShelterID = shelterId
        };

        await _repositoryWrapper.ShelterAdopterVerificationRepository.Create(newVerification);
        await _repositoryWrapper.Save();

        return ServiceResponse.Ok();
    }

    public async Task<ServiceResponse> CheckIfAdopterIsVerified(Guid adopterId, Guid shelterId)
    {
        var verification = (await _repositoryWrapper.ShelterAdopterVerificationRepository.FindByCondition(v =>
                       v.AdopterID == adopterId && v.ShelterID == shelterId)).SingleOrDefault();

        return ServiceResponse.Ok(verification is not null);
    }
}