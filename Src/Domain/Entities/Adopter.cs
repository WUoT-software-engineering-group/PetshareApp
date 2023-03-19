using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class Adopter : User
{
    [NotMapped]
    public virtual List<Announcement> FollowedAnnouncements { get; set; } = new List<Announcement>();

    [NotMapped]
    public virtual List<Announcement> FinalizedAnnouncements { get; set; } = new List<Announcement>();
}