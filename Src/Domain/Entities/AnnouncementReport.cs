namespace Petshare.Domain.Entities;

public class AnnouncementReport : Report
{
    public Guid AnnouncementID { get; set; }

    public Guid ShelterID { get; set; }
}
