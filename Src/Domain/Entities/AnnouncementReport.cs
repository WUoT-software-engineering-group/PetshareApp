namespace Petshare.Domain.Entities;

public class AnnouncementReport : Report
{
    public Guid AnnouncementID { get; private set; }

    public Guid ShelterID { get; private set; }

    public AnnouncementReport(Guid announcementID, Guid shelterID, string reason) : base(reason)
    {
        AnnouncementID = announcementID;
        ShelterID = shelterID;
    }
}
