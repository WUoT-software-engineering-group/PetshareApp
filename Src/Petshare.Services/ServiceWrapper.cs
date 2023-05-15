using Azure.Storage.Blobs;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class ServiceWrapper : IServiceWrapper
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private IShelterService? _shelterService;
    private IPetService? _petService;
    private IAnnouncementService? _announcementService;
    private IAdopterService? _adopterService;
    private IApplicationsService? _applicationsService;
    private readonly Lazy<IFileService> _lazyFileService;
    private readonly Lazy<IMailService> _lazyMailService;

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
            _announcementService ??= new AnnouncementService(_repositoryWrapper);
            return _announcementService;
        }
    }

    public IAdopterService AdopterService
    {
        get
        {
            _adopterService ??= new AdopterService(_repositoryWrapper);
            return _adopterService;
        }
    }
    
    public IApplicationsService ApplicationsService
    {
        get
        {
            _applicationsService ??= new ApplicationsService(_repositoryWrapper, this);
            return _applicationsService;
        }
    }

    public IFileService FileService => _lazyFileService.Value;

    public IMailService MailService => _lazyMailService.Value;

    public ServiceWrapper(IRepositoryWrapper repositoryWrapper, BlobServiceClient blobService, IServicesConfiguration configuration)
    {
        _repositoryWrapper = repositoryWrapper;
        _lazyFileService = new Lazy<IFileService>(() => 
            new FileService(blobService.GetBlobContainerClient(configuration.BlobContainerName)));
        _lazyMailService = new Lazy<IMailService>(() =>
            new MailService(configuration.MailApiKey, configuration.MailAddress, configuration.MailName));
    }
}