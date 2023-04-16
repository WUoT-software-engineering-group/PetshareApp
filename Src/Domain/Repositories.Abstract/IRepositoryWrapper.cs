using Petshare.Domain.Entities;

namespace Petshare.Domain.Repositories.Abstract;

public interface IRepositoryWrapper
{
    IRepository<Shelter> ShelterRepository { get; }

    IRepository<Pet> PetRepository { get; }

    IRepository<Announcement> AnnouncementRepository { get; }

    IRepository<Adopter> AdopterRepository { get; }

    IRepository<ShelterAdopterVerification> ShelterAdopterVerificationRepository { get; }

    Task Save();
}