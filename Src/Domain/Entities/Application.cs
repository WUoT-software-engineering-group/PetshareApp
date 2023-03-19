namespace Petshare.Domain.Entities;

public class Application
{
    public Guid ID { get; set; }

    public virtual User User { get; set; } = default!;

    public virtual Announcement Announcement { get; set; } = default!;

    public DateTime DateOfApplication { get; set; }

    public bool IsWithdrawn { get; set; }
}
