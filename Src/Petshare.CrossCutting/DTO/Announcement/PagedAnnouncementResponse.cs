namespace Petshare.CrossCutting.DTO.Announcement;
public class PagedAnnouncementResponse
{
    public List<LikedAnnouncementResponse> Announcements { get; set; } = default!;
    public int PageNumber { get; set; }
    public int Count { get; set; }
}
