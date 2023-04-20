namespace Petshare.CrossCutting.DTO.Pet
{
    public class PetResponse
    {
        public Guid ID { get; set; }

        public Guid ShelterID { get; set; }

        public string Name { get; set; } = default!;

        public string Species { get; set; } = default!;

        public string Breed { get; set; } = default!;

        public DateTime Birthday { get; set; }

        public string Description { get; set; } = default!;
        
        public string? PhotoUri { get; set; }
    }
}
