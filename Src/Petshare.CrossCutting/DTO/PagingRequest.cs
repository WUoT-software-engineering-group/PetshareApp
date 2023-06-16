namespace Petshare.CrossCutting.DTO;
public class PagingRequest
{
    public int PageNumber { get; set; } = 0;
    public int PageCount { get; set; } = 10;
}
