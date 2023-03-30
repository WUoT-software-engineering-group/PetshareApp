using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Announcement;

public class PutAnnouncementRequest
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public AnnouncementStatus Status { get; set; }
}