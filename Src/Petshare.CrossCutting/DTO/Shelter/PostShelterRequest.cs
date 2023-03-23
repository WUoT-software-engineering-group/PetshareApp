using Petshare.CrossCutting.DTO.Address;

namespace Petshare.CrossCutting.DTO.Shelter;

public class PostShelterRequest
{
    public string UserName { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string FullShelterName { get; set; } = default!;

    public AddressRequest Address { get; set; } = default!;
}