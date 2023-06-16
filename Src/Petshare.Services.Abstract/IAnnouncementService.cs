using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Utils;

namespace Petshare.Services.Abstract;

public interface IAnnouncementService
{
    Task<ServiceResponse> Create(Guid shelterId, PostAnnouncementRequest announcement);

    Task<ServiceResponse> Update(Guid userId, string? role, Guid announcementId, PutAnnouncementRequest announcement);

    Task<ServiceResponse> GetById(Guid announcementId);

    Task<ServiceResponse> GetByShelter(Guid shelterId, PagingRequest pagingRequest);

    Task<ServiceResponse> GetByFilters(GetAnnouncementsRequest filters, Guid? adopterId);

    Task<ServiceResponse> UpdateLikedStatus(Guid adopterId, Guid announcementId, bool isLiked);
}