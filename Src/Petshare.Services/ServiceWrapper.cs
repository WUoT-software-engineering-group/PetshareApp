using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class ServiceWrapper : IServiceWrapper
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private IShelterService? _shelterService;
    private IPetService? _petService;
    private IAnnouncementService? _announcementService;

    public IShelterService ShelterService
    {
        get
        {
            _shelterService ??= new ShelterService(_repositoryWrapper);
            return _shelterService;
        }
    }

    public IPetService PetService
    {
        get
        {
            _petService ??= new PetService(_repositoryWrapper);
            return _petService;
        }
    }

    public IAnnouncementService AnnouncementService
    {
        get
        {
            _announcementService ??= new AnnouncementService(_repositoryWrapper, PetService);
            return _announcementService;
        }
    }

    public ServiceWrapper(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }
}