using Petshare.CrossCutting.DTO.Address;

namespace Petshare.CrossCutting.DTO.Shelter
{
    public class ShelterResponse
    {
        public Guid ID { get; set; }

        public string UserName { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string FullShelterName { get; set; } = default!;

        public bool IsAuthorized { get; set; } = false;

        public AddressResponse Address { get; set; } = default!;
    }
}