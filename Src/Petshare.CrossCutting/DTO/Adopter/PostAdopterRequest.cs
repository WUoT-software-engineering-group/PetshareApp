using Petshare.CrossCutting.DTO.Address;

namespace Petshare.CrossCutting.DTO.Adopter;

public class PostAdopterRequest
{
    public string UserName { get; set; } = default!;

    public string? PhoneNumber { get; set; }

    public string Email { get; set; } = default!;

    public AddressRequest? Address { get; set; }
}