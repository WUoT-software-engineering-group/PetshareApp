namespace Petshare.Domain.Entities;

public class Application
{
    public Guid ID { get; private set; }
    public User User { get; private set; }
    public Announcement Announcement { get; private set; }
    public DateTime DateOfApplication { get; private set; }
    public bool IsWithdrawed { get; private set; }

    public Application(User user, Announcement announcement, DateTime dateOfApplication)
    {
        ID = Guid.NewGuid();
        User = user;
        Announcement = announcement;
        DateOfApplication = dateOfApplication;
        IsWithdrawed = false;
    }

    public Application() { }
}
