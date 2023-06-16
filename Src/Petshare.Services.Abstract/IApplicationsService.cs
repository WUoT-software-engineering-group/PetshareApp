using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.CrossCutting.Enums;
using Petshare.CrossCutting.Utils;

namespace Petshare.Services.Abstract;

public interface IApplicationsService
{
    Task<ServiceResponse> Create(Guid announcementId, Guid userId);

    Task<ServiceResponse> GetAll(string role, Guid userId, PagingRequest pagingRequest);

    Task<ServiceResponse> GetByAnnouncement(Guid announcementId, Guid shelterId, PagingRequest pagingRequest);

    Task<ServiceResponse> UpdateStatus(Guid applicationId, ApplicationStatus status, Guid shelterId);
}
