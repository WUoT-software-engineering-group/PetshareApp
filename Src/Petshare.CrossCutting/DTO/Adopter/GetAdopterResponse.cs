using Petshare.CrossCutting.DTO.Address;
using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Adopter;

public class GetAdopterResponse
{
    public Guid ID { get; set; }

    public string? UserName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public AddressResponse? Address { get; set; }

    public AdopterStatus Status { get; set; }
}