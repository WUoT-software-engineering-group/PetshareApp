namespace Petshare.Domain.Entities;

public class Pet
{
    public Guid ID { get; set; }

    public Shelter Shelter { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Species { get; set; } = default!;

    public string Breed { get; set; } = default!;

    public DateTime Birthday { get; set; }

    public string Description { get; set; } = default!;

    public byte[] Photo { get; set; } = default!;
}
