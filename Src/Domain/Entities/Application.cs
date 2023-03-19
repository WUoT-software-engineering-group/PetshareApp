namespace Petshare.Domain.Entities;

public class Application
{
    public Guid ID { get; set; }

    public User User { get; set; } = default!;

    public Announcement Announcement { get; set; } = default!;

    public DateTime DateOfApplication { get; set; }

    public bool IsWithdrawn { get; set; }
}
