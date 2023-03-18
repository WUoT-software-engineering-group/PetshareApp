namespace Petshare.Domain.Entities;

public class ShelterReport : Report
{
    public Guid ShelterID { get; private set; }

    public ShelterReport(Guid shelterID, string reason) : base(reason)
    {
        ShelterID = shelterID;
    }
}


