﻿using Mapster;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.CrossCutting.Enums;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;
using System.Data;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Utils;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.DTO;

namespace Petshare.Services;

public class ApplicationsService : IApplicationsService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IServiceWrapper _serviceWrapper;

    public ApplicationsService(IRepositoryWrapper repositoryWrapper, IServiceWrapper serviceWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _serviceWrapper = serviceWrapper;
    }

    public async Task<ServiceResponse> Create(Guid announcementId, Guid userId)
    {
        var announcement = (await _repositoryWrapper.AnnouncementRepository.FindByCondition(x => x.ID == announcementId)).FirstOrDefault();
        var adopter = (await _repositoryWrapper.AdopterRepository.FindByCondition(x => x.ID == userId)).FirstOrDefault();

        if (announcement is null || adopter is null)
        {
            return ServiceResponse.BadRequest();
        }

        var applicationToAdd = new Application
        {
            Adopter = adopter,
            Announcement = announcement
        };

        var application = await _repositoryWrapper.ApplicationsRepository.Create(applicationToAdd);
        await _repositoryWrapper.Save();

        return ServiceResponse.Created(application.ID);
    }

    public async Task<ServiceResponse> GetAll(string role, Guid userId, PagingRequest pagingRequest)
    {
        var applications = role switch
        {
            "admin" => await _repositoryWrapper.ApplicationsRepository.FindAll(),
            "adopter" => await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.Adopter.ID == userId),
            "shelter" => await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.Announcement.Author.ID == userId),
            _ => throw new NotImplementedException(),
        };

        return ServiceResponse.Ok(new PagedApplicationResponse
        {
            Applications = applications.Skip(pagingRequest.PageNumber * pagingRequest.PageCount).Take(pagingRequest.PageCount).Adapt<List<ApplicationResponse>>(),
            PageNumber = pagingRequest.PageNumber,
            Count = applications.Count()
        });
    }

    public async Task<ServiceResponse> GetByAnnouncement(Guid announcementId, Guid shelterId, PagingRequest pagingRequest)
    {
        var result = await _serviceWrapper.AnnouncementService.GetById(announcementId);

        if (result.StatusCode.NotFound() || (result.Data as AnnouncementResponse)!.Pet.Shelter.ID != shelterId)
        {
            return ServiceResponse.BadRequest();
        }

        var applications = await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.Announcement.ID == announcementId);

        return ServiceResponse.Ok(new PagedApplicationResponse
        {
            Applications = applications.Skip(pagingRequest.PageNumber * pagingRequest.PageCount).Take(pagingRequest.PageCount).Adapt<List<ApplicationResponse>>(),
            PageNumber = pagingRequest.PageNumber,
            Count = applications.Count()
        });
    }

    public async Task<ServiceResponse> UpdateStatus(Guid applicationId, ApplicationStatus status, Guid shelterId)
    {
        var application = (await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.ID == applicationId)).FirstOrDefault();

        if (application is null || application.Announcement.Author.ID != shelterId)
        {
            return ServiceResponse.BadRequest();
        }

        application.Status = status;
        application.LastUpdateDate = DateTime.Now;
        await _repositoryWrapper.Save();

        if (status is ApplicationStatus.Accepted or ApplicationStatus.Rejected)
            await _serviceWrapper.MailService.SendApplicationDecisionMail(application.Adopter.UserName,
                application.Adopter.Email, status, application.Announcement.Pet.Name, application.Announcement.Pet.Breed);

        return ServiceResponse.Ok();
    }
}
