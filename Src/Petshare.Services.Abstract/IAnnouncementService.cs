using Petshare.CrossCutting.DTO.Announcement;

namespace Petshare.Services.Abstract;

public interface IAnnouncementService
{
    Task<AnnouncementResponse?> Create(Guid shelterId, PostAnnouncementRequest announcement);

    Task<bool> Update(Guid userId, Guid announcementId, PutAnnouncementRequest announcement);

    Task<AnnouncementResponse?> GetById(Guid announcementId);
    Task<List<AnnouncementResponse>> GetByShelter(Guid shelterId);
    Task<List<AnnouncementResponse>> GetByFilters(GetAnnouncementsRequest filters);
}