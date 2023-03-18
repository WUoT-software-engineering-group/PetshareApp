namespace Petshare.Domain.Entities;

public class Pet
{
    public Guid ID { get; private set; }
    public Shelter Shelter { get; private set; }
    public string Name { get; private set; }
    public string Species { get; private set; }
    public string Breed { get; private set; }
    public DateTime Birthday { get; private set; }
    public string Description { get; private set; }
    public byte[] Photo { get; private set; }

    public Pet(Shelter shelter, string name, string species, string breed, DateTime birthday, string description, byte[] photo)
    {
        ID = Guid.NewGuid();
        Shelter = shelter;
        Name = name;
        Species = species;
        Breed = breed;
        Birthday = birthday;
        Description = description;
        Photo = photo;
    }

    public Pet() { }
}
