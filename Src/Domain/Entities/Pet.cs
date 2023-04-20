using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Petshare.Domain.Entities;

public class Pet
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid ID { get; set; }

    public virtual Shelter Shelter { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Species { get; set; } = default!;

    public string Breed { get; set; } = default!;

    public DateTime Birthday { get; set; }

    public string Description { get; set; } = default!;

    public string? PhotoUri { get; set; }
}
