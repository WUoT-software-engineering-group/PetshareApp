using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class Adopter : User
{
    [NotMapped]
    public List<Annoucement> FollowedAnnoucements { get; private set; }
    [NotMapped]
    public List<Annoucement> FinalisedAnnoucements { get; private set; }

    public Adopter(string userName, string phoneNumber, string email, Address address, AnnoucementProvider annoucementProvider)
        : base(userName, phoneNumber, email, address, annoucementProvider)
    {
        FollowedAnnoucements = new List<Annoucement>();
        FinalisedAnnoucements = new List<Annoucement>();
    }

    public Adopter() { }
}