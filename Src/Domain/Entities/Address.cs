using Microsoft.EntityFrameworkCore;

namespace Petshare.Domain.Entities;

[Owned]
public class Address
{
    public string Street { get; set; } = default!;

    public string City { get; set; } = default!;

    public string? Province { get; set; }

    public string PostalCode { get; set; } = default!;

    public string Country { get; set; } = default!;
}