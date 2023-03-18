namespace Petshare.Domain.Entities;

public class Application
{
    public Guid ID { get; private set; }
    public User User { get; private set; }
    public Annoucement Annoucement { get; private set; }
    public DateTime DateOfApplication { get; private set; }
    public bool IsWithdrawed { get; private set; }

    public Application(User user, Annoucement annoucement, DateTime dateOfApplication)
    {
        ID = Guid.NewGuid();
        User = user;
        Annoucement = annoucement;
        DateOfApplication = dateOfApplication;
        IsWithdrawed = false;
    }

    public Application() { }
}
