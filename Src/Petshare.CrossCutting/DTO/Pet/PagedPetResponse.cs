namespace Petshare.CrossCutting.DTO.Pet;

public class PagedPetResponse : PagedBaseResponse
{
    public List<PetResponse> Pets { get; set; } = default!;
}
