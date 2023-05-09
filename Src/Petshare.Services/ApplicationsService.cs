using Mapster;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

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

    public async Task<Guid?> Create(Guid announcementId)
    {
        var announcement = (await _repositoryWrapper.AnnouncementRepository.FindByCondition(x => x.ID == announcementId)).FirstOrDefault();

        if (announcement is null)
        {
            return null;
        }

        var application = await _repositoryWrapper.ApplicationsRepository
            .Create(new Application
            {
                Announcement = announcement
            });
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
}
