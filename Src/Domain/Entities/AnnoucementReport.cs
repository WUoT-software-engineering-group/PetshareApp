namespace Petshare.Domain.Entities;

public class AnnoucementReport : Report
{
    public Guid AnnoucementID { get; private set; }

    public Guid ShelterID { get; private set; }

    public AnnoucementReport(Guid annoucementID, Guid shelterID, string reason) : base(reason)
    {
        AnnoucementID = annoucementID;
        ShelterID = shelterID;
    }
}
