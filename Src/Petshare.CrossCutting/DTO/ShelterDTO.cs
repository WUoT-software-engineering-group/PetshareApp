namespace Petshare.CrossCutting.DTO
{
    public class ShelterDTO
    {
        public Guid ID { get; set; }

        public string UserName { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Email { get; set; } = default!;

        public Guid AddressId { get; set; }

        public string FullShelterName { get; set; } = default!;

        public bool IsAuthorized { get; set; } = false;

        public AddressDTO Address { get; set; } = default!;
    }
}