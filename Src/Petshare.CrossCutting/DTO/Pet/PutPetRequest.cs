using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Pet
{
    public class PutPetRequest
    {
        public string Name { get; set; } = default!;

        public string Species { get; set; } = default!;

        public string Breed { get; set; } = default!;

        public DateTime Birthday { get; set; }

        public string Description { get; set; } = default!;

        public PetStatus Status { get; set; }
    }
}
