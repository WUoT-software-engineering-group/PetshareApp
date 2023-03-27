using Petshare.CrossCutting.DTO.Pet;

namespace Petshare.CrossCutting.DTO.Announcement;

public class PostAnnouncementRequest
{
    public string Title { get; set; } = default!;

    public string Description { get; set; } = default!;

    public Guid? PetId { get; set; }

    public PostPetRequest? Pet { get; set; }
}