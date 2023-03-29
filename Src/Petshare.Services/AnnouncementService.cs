﻿namespace Petshare.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public AnnouncementService(IRepositoryWrapper repositoryWrapper)
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
            if (shelterId != pet.Shelter.ID)
                return null;
            announcementToCreate.Author = pet.Shelter;
        }
        else if (announcement.Pet is not null)
        {
            var petToCreate = announcement.Pet.Adapt<Pet>();
            petToCreate.ID = Guid.NewGuid();

            var petShelter = (await _repositoryWrapper.ShelterRepository.FindByCondition(s => s.ID == shelterId))
                .SingleOrDefault();
            if (petShelter is null)
                return null;

            petToCreate.Shelter = petShelter;

            announcementToCreate.Pet = petToCreate;
            announcementToCreate.Author = petShelter;
        }
        else
            return null;

        var createdAnnouncement = await _repositoryWrapper.AnnouncementRepository.Create(announcementToCreate);
        await _repositoryWrapper.Save();

        return createdAnnouncement.Adapt<AnnouncementResponse>();
    }

    public async Task<bool> Update(Guid userId, Guid announcementId, PutAnnouncementRequest announcement)
    {
        // TODO: dodać weryfikację czy użytkownik ma uprawnienia do edycji ogłoszenia
        var announcementToUpdate = (await _repositoryWrapper.AnnouncementRepository.FindByCondition(a => a.ID == announcementId))
            .SingleOrDefault();

        if (announcementToUpdate is null)
            return false;

        announcementToUpdate = announcement.Adapt(announcementToUpdate);
        await _repositoryWrapper.AnnouncementRepository.Update(announcementToUpdate);
        await _repositoryWrapper.Save();

        return true;
    }

    public async Task<AnnouncementResponse?> GetById(Guid announcementId)
    {
        var announcements = await _repositoryWrapper.AnnouncementRepository.FindByCondition(x => x.ID == announcementId);

        if (announcements.SingleOrDefault() is not Announcement announcement)
        {
            return null;
        }

        return announcement.Adapt<AnnouncementResponse>();
    }

    public async Task<List<AnnouncementResponse>> GetByFilters(GetAnnouncementsRequest filters)
    {
        Expression<Func<Announcement, bool>> condition = _ => true;

        if (!filters.Breeds.IsNullOrEmpty())
        {
            condition = condition.And(a => filters.Breeds.Contains(a.Pet.Breed));
        }

        if (!filters.Species.IsNullOrEmpty())
        {
            condition = condition.And(a => filters.Species.Contains(a.Pet.Species));
        }

        if (!filters.Cities.IsNullOrEmpty())
        {
            condition = condition.And(a => filters.Cities.Contains(a.Pet.Shelter.Address.City));
        }

        if (!filters.ShelterNames.IsNullOrEmpty())
        {
            condition = condition.And(a => filters.ShelterNames.Contains(a.Pet.Shelter.FullShelterName));
        }

        if (filters.MinAge.HasValue)
        {
            condition = condition.And(a => a.Pet.Birthday.AddYears(filters.MinAge.Value).CompareTo(DateTime.Now) <= 0);
        }

        if (filters.MaxAge.HasValue)
        {
            condition = condition.And(a => a.Pet.Birthday.AddYears(filters.MaxAge.Value).CompareTo(DateTime.Now) >= 0);
        }

        var announcements = await _repositoryWrapper.AnnouncementRepository.FindByCondition(condition);
        return announcements.Adapt<List<AnnouncementResponse>>();
    }
}