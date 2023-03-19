using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class Adopter : User
{
    [NotMapped]
    public List<Announcement> FollowedAnnouncements { get; private set; }
    [NotMapped]
    public List<Announcement> FinalizedAnnouncements { get; private set; }

    public Adopter(string userName, string phoneNumber, string email, Address address, AnnouncementProvider announcementProvider)
        : base(userName, phoneNumber, email, address, announcementProvider)
    {
        FollowedAnnouncements = new List<Announcement>();
        FinalizedAnnouncements = new List<Announcement>();
    }

    public Adopter() { }
}