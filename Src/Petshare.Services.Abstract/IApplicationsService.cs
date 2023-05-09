using Petshare.CrossCutting.DTO.Applications;

namespace Petshare.Services.Abstract;

public interface IApplicationsService
{
    Task<Guid?> Create(Guid announcementId);

    Task<List<ApplicationResponse>> GetAll(string role, Guid userId);
}
