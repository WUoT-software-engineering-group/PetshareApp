using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class ServiceWrapper : IServiceWrapper
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private IShelterService? _shelterService;

    public IShelterService ShelterService
    {
        get
        {
            _shelterService ??= new ShelterService(_repositoryWrapper);
            return _shelterService;
        }
    }

    public ServiceWrapper(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }
}