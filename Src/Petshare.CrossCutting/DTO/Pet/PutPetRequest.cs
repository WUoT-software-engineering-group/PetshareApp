using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Pet
{
    public class PutPetRequest
    {
        public string? Name { get; set; }

        public string? Species { get; set; }

        public string? Breed { get; set; }

        public DateTime? Birthday { get; set; }

        public string? Description { get; set; }

        public PetStatus? Status { get; set; }

        public Sex? Sex { get; set; }
    }
}
