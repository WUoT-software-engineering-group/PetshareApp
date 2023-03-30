using Mapster;
using Petshare.CrossCutting.DTO.Pet;
using Petshare.Domain.Entities;

namespace Petshare.Services.Mapping
{
    public class PetMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Pet, PetResponse>()
                .Map(dest => dest.ShelterID, src => src.Shelter.ID);
        }
    }
}
