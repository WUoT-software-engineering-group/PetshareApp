namespace Petshare.Domain.Entities;

public enum AnnoucementStatus
{
    Open,
    Closed,
    Verification
}

public class Annoucement
{
    public Guid ID { get; private set; }
    public Shelter Author { get; private set; }
    public Pet Pet { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime CreationDate { get; private set; }
    public DateTime ClosingDate { get; private set; }
    public AnnoucementStatus Status { get; set; }

    public Annoucement(Shelter author, Pet pet, string title, string description, DateTime creationDate, DateTime closingDate)
    {
        ID = Guid.NewGuid();
        Author = author;
        Pet = pet;
        Title = title;
        Description = description;
        CreationDate = creationDate;
        ClosingDate = closingDate;
        Status = AnnoucementStatus.Open;
    }

    public Annoucement() { }
}

public abstract class AdopterAnnoucement
{
    public Guid AdopterID { get; private set; }
    public Adopter Adopter { get; private set; }
    public Guid AnnoucementID { get; private set; }
    public Annoucement annoucement { get; private set; }

    public AdopterAnnoucement() { }
}

public class AdopterAnnoucementFollowed : AdopterAnnoucement
{
    public AdopterAnnoucementFollowed() { }
}

public class AdopterAnnoucementFinalised : AdopterAnnoucement
{
    public AdopterAnnoucementFinalised() { }
}