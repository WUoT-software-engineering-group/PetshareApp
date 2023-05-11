using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Announcement;

public class AnnouncementResponse
{
    public Guid ID { get; set; }

    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public DateTime CreationDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public AnnouncementStatus Status { get; set; }

    public PetResponse Pet { get; set; } = default!;
}