namespace Petshare.Domain.Entities;

public abstract class Report
{
    public Guid ID { get; private set; }
    public string Reason { get; private set; }

    public Report(string reason)
    {
        ID = Guid.NewGuid();
        Reason = reason;
    }

    public Report() { }
}
