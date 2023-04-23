using Petshare.CrossCutting.Enums;

namespace Petshare.CrossCutting.DTO.Adopter;

public class PutAdopterRequest
{
    public AdopterStatus Status { get; set; }
}