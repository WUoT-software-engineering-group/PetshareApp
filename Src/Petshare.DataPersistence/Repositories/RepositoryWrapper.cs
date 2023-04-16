using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.DataPersistence.Repositories;

public class RepositoryWrapper : IRepositoryWrapper, IDisposable
{
    private readonly PetshareDbContext _repositoryDbContext;
    private IRepository<Shelter>? _shelterRepository;
    private IRepository<Pet>? _petRepository;
    private IRepository<Announcement>? _announcementRepository;
    private IRepository<Adopter>? _adopterRepository;
    private IRepository<ShelterAdopterVerification>? _shelterAdopterVerificationRepository;

    public IRepository<Shelter> ShelterRepository
    {
        get
        {
            _shelterRepository ??= new Repository<Shelter>(_repositoryDbContext);
            return _shelterRepository;
        }
    }

    public IRepository<Pet> PetRepository
    {
        get
        {
            _petRepository ??= new Repository<Pet>(_repositoryDbContext);
            return _petRepository;
        }
    }

    public IRepository<Announcement> AnnouncementRepository
    {
        get
        {
            _announcementRepository ??= new Repository<Announcement>(_repositoryDbContext);
            return _announcementRepository;
        }
    }

    public IRepository<Adopter> AdopterRepository
    {
        get
        {
            _adopterRepository ??= new Repository<Adopter>(_repositoryDbContext);
            return _adopterRepository;
        }
    }

    public IRepository<ShelterAdopterVerification> ShelterAdopterVerificationRepository
    {
        get
        {
            _shelterAdopterVerificationRepository ??= new Repository<ShelterAdopterVerification>(_repositoryDbContext);
            return _shelterAdopterVerificationRepository;
        }
    }

    public RepositoryWrapper(PetshareDbContext repositoryDbContext)
    {
        _repositoryDbContext = repositoryDbContext;
    }

    public async Task Save()
    {
        await _repositoryDbContext.SaveChangesAsync();
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _repositoryDbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}