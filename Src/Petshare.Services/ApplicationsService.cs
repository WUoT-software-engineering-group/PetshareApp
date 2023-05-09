using Mapster;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.CrossCutting.Enums;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;
using System.Data;

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

    public async Task<Guid?> Create(Guid announcementId, Guid userId)
    {
        var announcement = (await _repositoryWrapper.AnnouncementRepository.FindByCondition(x => x.ID == announcementId)).FirstOrDefault();
        var user = (await _repositoryWrapper.AdopterRepository.FindByCondition(x => x.ID == userId)).FirstOrDefault();

        if (announcement is null || user is null)
        {
            return null;
        }

        var applicationToAdd = new Application
        {
            User = user,
            Announcement = announcement
        };

        var application = await _repositoryWrapper.ApplicationsRepository.Create(applicationToAdd);
        await _repositoryWrapper.Save();

        return application.ID;
    }

    public async Task<List<ApplicationResponse>> GetAll(string role, Guid userId)
    {
        var applications = role switch
        {
            "admin" => await _repositoryWrapper.ApplicationsRepository.FindAll(),
            "adopter" => await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.User.ID == userId),
            "shelter" => await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.Announcement.Author.ID == userId),
            _ => throw new NotImplementedException(),
        };

        return applications.Adapt<List<ApplicationResponse>>();
    }

    public async Task<List<ApplicationResponse>?> GetByAnnouncement(Guid announcementId, Guid shelterId)
    {
        var announcement = await _serviceWrapper.AnnouncementService.GetById(announcementId);

        if (announcement is null || announcement.Author.ID != shelterId)
        {
            return null;
        }

        var applications = await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.Announcement.ID == announcementId);

        return applications.Adapt<List<ApplicationResponse>>();
    }

    public async Task<bool> UpdateStatus(Guid applicationId, ApplicationStatus status, Guid shelterId)
    {
        var application = (await _repositoryWrapper.ApplicationsRepository.FindByCondition(x => x.ID == applicationId)).FirstOrDefault();

        if (application is null || application.Announcement.Author.ID != shelterId)
        {
            return false;
        }

        application.Status = status;
        await _repositoryWrapper.Save();

        return true;
    }
}
