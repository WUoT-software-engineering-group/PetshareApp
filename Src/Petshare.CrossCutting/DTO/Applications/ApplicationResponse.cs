using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Applications;

public class ApplicationResponse
{
    public Guid ID { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public Guid AnnouncementId { get; set; }

    public AnnouncementResponse Announcement { get; set; } = default!;

    public GetAdopterResponse Adopter { get; set; } = default!;

    public ApplicationStatus ApplicationStatus { get; set; }
}
