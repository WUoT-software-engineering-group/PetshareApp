namespace Petshare.CrossCutting.DTO.Announcement;
public class PagedAnnouncementResponse : PagedBaseResponse
{
    public List<LikedAnnouncementResponse> Announcements { get; set; } = default!;
}
