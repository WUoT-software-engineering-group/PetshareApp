using Petshare.CrossCutting.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class Application
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid ID { get; set; }

    public virtual User User { get; set; } = default!;

    public virtual Announcement Announcement { get; set; } = default!;

    public DateTime DateOfApplication { get; set; }

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Created;
}
