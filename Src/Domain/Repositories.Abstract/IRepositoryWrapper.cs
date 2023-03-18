using Petshare.Domain.Entities;

namespace Petshare.Domain.Repositories.Abstract;

public interface IRepositoryWrapper
{
    IRepository<Shelter> ShelterRepository { get; }

    Task Save();
}