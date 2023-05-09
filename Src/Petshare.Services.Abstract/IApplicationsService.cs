using Petshare.CrossCutting.DTO.Applications;

namespace Petshare.Services.Abstract;

public interface IApplicationsService
{
    Task<List<ApplicationResponse>> GetAll(string role, Guid userId);
}
