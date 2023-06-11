namespace Petshare.CrossCutting.DTO.Shelter;

public class PagedShelterResponse : PagedBaseResponse
{
    public List<ShelterResponse> Shelters { get; set; } = default;
}
