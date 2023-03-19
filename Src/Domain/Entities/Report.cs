namespace Petshare.Domain.Entities;

public abstract class Report
{
    public Guid ID { get; set; }

    public string Reason { get; set; } = default!;
}
