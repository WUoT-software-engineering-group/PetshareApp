﻿using Mapster;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Enums;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;
using Petshare.CrossCutting.Utils;
using System.Linq.Expressions;

namespace Petshare.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public AnnouncementService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public async Task<ServiceResponse> Create(Guid shelterId, PostAnnouncementRequest announcement)
    {
        var announcementToCreate = announcement.Adapt<Announcement>();
        announcementToCreate.Status = AnnouncementStatus.Open;

        var pet = (await _repositoryWrapper.PetRepository.FindByCondition(p => p.ID == announcement.PetId))
            .SingleOrDefault();
        if (pet == null || shelterId != pet.Shelter.ID)
            return ServiceResponse.BadRequest();

        announcementToCreate.Pet = pet;
        announcementToCreate.Author = pet.Shelter;

        var createdAnnouncement = await _repositoryWrapper.AnnouncementRepository.Create(announcementToCreate);
        await _repositoryWrapper.Save();

        return ServiceResponse.Created(createdAnnouncement.ID);
    }

    public async Task<ServiceResponse> Update(Guid userId, string? role, Guid announcementId, PutAnnouncementRequest announcement)
    {
        var announcementToUpdate = (await _repositoryWrapper.AnnouncementRepository.FindByCondition(a => a.ID == announcementId))
            .SingleOrDefault();

        if (announcementToUpdate is null 
            || (role == "shelter" && announcementToUpdate.Author.ID != userId))
            return ServiceResponse.BadRequest();

        announcementToUpdate = announcement.Adapt(announcementToUpdate);
        await _repositoryWrapper.AnnouncementRepository.Update(announcementToUpdate);
        await _repositoryWrapper.Save();

        return ServiceResponse.Ok();
    }

    public async Task<ServiceResponse> GetById(Guid announcementId)
    {
        var announcements = await _repositoryWrapper.AnnouncementRepository.FindByCondition(x => x.ID == announcementId);

        if (announcements.SingleOrDefault() is not Announcement announcement)
        {
            return ServiceResponse.NotFound();
        }

        return ServiceResponse.Ok(announcement.Adapt<AnnouncementResponse>());
    }

    public async Task<ServiceResponse> GetByShelter(Guid shelterId)
    {
        var announcements = await _repositoryWrapper.AnnouncementRepository.FindByCondition(x => x.Pet.Shelter.ID == shelterId);
        return ServiceResponse.Ok(announcements.Adapt<List<AnnouncementResponse>>());
    }

    public async Task<ServiceResponse> GetByFilters(GetAnnouncementsRequest filters)
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
        return ServiceResponse.Ok(announcements.Adapt<List<AnnouncementResponse>>());
    }
}