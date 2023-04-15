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
    private readonly Lazy<IFileService> _lazyFileService;

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

    public IFileService FileService => _lazyFileService.Value;

    public ServiceWrapper(IRepositoryWrapper repositoryWrapper, BlobServiceClient blobService, IServicesConfiguration configuration)
    {
        _repositoryWrapper = repositoryWrapper;
        _lazyFileService = new Lazy<IFileService>(() => 
            new FileService(blobService.GetBlobContainerClient(configuration.BlobContainerName)));
    }
}