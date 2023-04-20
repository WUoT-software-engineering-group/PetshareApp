using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Pet
{
    public class PostPetRequest
    {
        public string Name { get; set; } = default!;

        public string Species { get; set; } = default!;

        public string Breed { get; set; } = default!;

        public DateTime Birthday { get; set; }

        public string Description { get; set; } = default!;

        public Sex Sex { get; set; }
    }
}
