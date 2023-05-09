using Mapster;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class ApplicationsService : IApplicationsService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public ApplicationsService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
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

        return applications.ToList().Adapt<List<ApplicationResponse>>();
    }
}
