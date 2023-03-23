namespace Petshare.Domain.Entities;

public enum AnnouncementStatus
{
    Open,
    Closed,
    Verification
}

public class Announcement
{
    public Guid ID { get; set; }

    public virtual Shelter Author { get; set; } = default!;

    public virtual Pet Pet { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreationDate { get; set; }

    public DateTime ClosingDate { get; set; }

    public AnnouncementStatus Status { get; set; }
}

public abstract class AdopterAnnouncement
{
    public Guid AdopterID { get; set; }

    public virtual Adopter Adopter { get; set; } = default!;

    public Guid AnnouncementID { get; set; }

    public virtual Announcement Announcement { get; set; } = default!;
}

public class AdopterAnnouncementFollowed : AdopterAnnouncement { }

public class AdopterAnnouncementFinalized : AdopterAnnouncement { }