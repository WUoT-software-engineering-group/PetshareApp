namespace Petshare.Domain.Entities;

public enum AnnouncementStatus
{
    Open,
    Closed,
    Verification
}

public class Announcement
{
    public Guid ID { get; private set; }
    public Shelter Author { get; private set; }
    public Pet Pet { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ClosingDate { get; private set; }
    public AnnouncementStatus Status { get; set; }

    public Announcement(Shelter author, Pet pet, string title, string description, DateTime creationDate, DateTime closingDate)
    {
        ID = Guid.NewGuid();
        Author = author;
        Pet = pet;
        Title = title;
        Description = description;
        CreationDate = creationDate;
        ClosingDate = closingDate;
        Status = AnnouncementStatus.Open;
    }

    public Announcement() { }
}

public abstract class AdopterAnnouncement
{
    public Guid AdopterID { get; private set; }
    public Adopter Adopter { get; private set; }
    public Guid AnnouncementID { get; private set; }
    public Announcement Announcement { get; private set; }

    public AdopterAnnouncement() { }
}

public class AdopterAnnouncementFollowed : AdopterAnnouncement
{
    public AdopterAnnouncementFollowed() { }
}

public class AdopterAnnouncementFinalized : AdopterAnnouncement
{
    public AdopterAnnouncementFinalized() { }
}