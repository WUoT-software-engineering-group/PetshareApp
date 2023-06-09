﻿using Petshare.CrossCutting.Enums;
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

    public string? Breed { get; set; }

    public DateTime Birthday { get; set; }

    public string? Description { get; set; }

    public string? PhotoUri { get; set; }

    public PetStatus Status { get; set; } = PetStatus.Active;

    public Sex Sex { get; set; } = Sex.Unknown;
}
