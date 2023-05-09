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
}
