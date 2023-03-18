namespace Petshare.Domain.Entities;

public class Shelter : User 
{
    public string FullShelterName { get; private set; }
    public bool IsAuthorized { get; private set; }
    public List<Pet> Pets { get; private set; }

    public Shelter(string fullShelterName, string userName, string phoneNumber, string email, Address address, AnnoucementProvider annoucementProvider)
        : base(userName, phoneNumber, email, address, annoucementProvider)
    {
        FullShelterName = fullShelterName;
        IsAuthorized = false;
        Pets = new List<Pet>();
    }
    public Shelter() { }
}
