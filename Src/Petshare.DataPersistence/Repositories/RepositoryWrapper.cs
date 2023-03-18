using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.WebAPI.Data;

namespace Petshare.DataPersistence.Repositories;

public class RepositoryWrapper : IRepositoryWrapper, IDisposable
{
    private readonly PetshareDbContext _repositoryDbContext;
    private IRepository<Shelter>? _shelterRepository;

    public IRepository<Shelter> ShelterRepository
    {
        get
        {
            _shelterRepository ??= new Repository<Shelter>(_repositoryDbContext);
            return _shelterRepository;
        }
    }

    public RepositoryWrapper(PetshareDbContext repositoryDbContext)
    {
        _repositoryDbContext = repositoryDbContext;
    }

    public void Save()
    {
        _repositoryDbContext.SaveChanges();
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