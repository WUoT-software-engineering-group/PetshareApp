namespace Petshare.Domain.Entities;

public class AdopterReport : Report
{
    public Guid AdopterID { get; private set; }

    public AdopterReport(Guid adopterID, string reason) : base(reason)
    {
        AdopterID = adopterID;
    }
}
