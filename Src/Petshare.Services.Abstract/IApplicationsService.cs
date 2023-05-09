using Petshare.CrossCutting.DTO.Applications;
using Petshare.CrossCutting.Enums;

namespace Petshare.Services.Abstract;

public interface IApplicationsService
{
    Task<Guid?> Create(Guid announcementId, Guid userId);

    Task<List<ApplicationResponse>> GetAll(string role, Guid userId);

    Task<List<ApplicationResponse>?> GetByAnnouncement(Guid announcementId, Guid shelterId);

    Task<bool> UpdateStatus(Guid applicationId, ApplicationStatus status, Guid shelterId);
}
