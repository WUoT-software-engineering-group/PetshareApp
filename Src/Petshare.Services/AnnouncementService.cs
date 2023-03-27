using Mapster;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Enums;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public AnnouncementService(IRepositoryWrapper repositoryWrapper, IPetService petService)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<AnnouncementResponse?> Create(Guid shelterId, PostAnnouncementRequest announcement)
    {
        var announcementToCreate = announcement.Adapt<Announcement>();
        announcementToCreate.ID = Guid.NewGuid();
        announcementToCreate.CreationDate = announcementToCreate.LastUpdateDate = DateTime.Now;
        announcementToCreate.Status = AnnouncementStatus.Open;

        if (announcement.PetId.HasValue)
        {
            var pet = (await _repositoryWrapper.PetRepository.FindByCondition(p => p.ID == announcement.PetId))
                .SingleOrDefault();
            if (pet == null)
                return null;

            announcementToCreate.Pet = pet;
        }
        else if (announcement.Pet is not null)
        {
            var petToCreate = announcement.Pet.Adapt<Pet>();
            petToCreate.ID = Guid.NewGuid();

            var petShelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(x => x.ID == shelterId))
                .SingleOrDefault();
            if (petShelter is null)
                return null;

            petToCreate.Shelter = petShelter;

            announcementToCreate.Pet = petToCreate;
        }
        else
            return null;

        var createdAnnouncement = await _repositoryWrapper.AnnouncementRepository.Create(announcementToCreate);
        await _repositoryWrapper.Save();

        return createdAnnouncement.Adapt<AnnouncementResponse>();
    }
}