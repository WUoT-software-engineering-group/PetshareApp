using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Announcement;

public class PutAnnouncementRequest
{
    public string? Title { get; set; }

    public string? Description { get; set; }

    public AnnouncementStatus? Status { get; set; }
}