using Petshare.CrossCutting.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class Application
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid ID { get; set; }

    public virtual Adopter Adopter { get; set; } = default!;

    public virtual Announcement Announcement { get; set; } = default!;

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Created;
}
