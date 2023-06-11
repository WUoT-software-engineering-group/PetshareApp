namespace Petshare.CrossCutting.DTO.Adopter;

public class PagedAdopterResponse : PagedBaseResponse
{
    public List<GetAdopterResponse> Adopters { get; set; } = default!;
}
