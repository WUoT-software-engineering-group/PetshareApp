namespace Petshare.CrossCutting.DTO.Address;

public class AddressRequest
{
    public string Street { get; set; } = default!;

    public string City { get; set; } = default!;

    public string Province { get; set; } = default!;

    public string PostalCode { get; set; } = default!;

    public string Country { get; set; } = default!;
}