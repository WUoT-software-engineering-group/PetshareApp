namespace Petshare.CrossCutting.DTO;

public abstract class PagedBaseResponse
{
    public int PageNumber { get; set; }
    public int Count { get; set; }
}
