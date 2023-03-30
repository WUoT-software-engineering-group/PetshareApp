using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public abstract class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid ID { get; set; }

    public string UserName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Email { get; set; } = default!;

    public virtual Address Address { get; set; } = default!;

    public virtual AnnouncementProvider AnnouncementProvider { get; set; } = default!;
}