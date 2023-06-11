namespace Petshare.CrossCutting.DTO.Applications;

public class PagedApplicationResponse : PagedBaseResponse
{
    public List<ApplicationResponse> Applications { get; set; } = default!;
}
