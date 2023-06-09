﻿using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Pet
{
    public class PetResponse
    {
        public Guid ID { get; set; }

        public ShelterResponse Shelter { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Species { get; set; } = default!;

        public string Breed { get; set; } = default!;

        public DateTime Birthday { get; set; }

        public string Description { get; set; } = default!;
        
        public string? PhotoUrl { get; set; }

        public PetStatus Status { get; set; }

        public Sex Sex { get; set; }
    }
}
