namespace Petshare.Domain.Entities;

public abstract class User
{
    public Guid ID { get; private set; }
    public string UserName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Email { get; private set; }
    public Address Address { get; private set; }
    public AnnoucementProvider AnnoucementProvider { get; private set; }

    public User(string userName, string phoneNumber, string email, Address address, AnnoucementProvider annoucementProvider)
    {
        ID = Guid.NewGuid();
        UserName = userName;
        PhoneNumber = phoneNumber;
        Email = email;
        Address = address;
        AnnoucementProvider = annoucementProvider;
    }

    public User() { }
}
