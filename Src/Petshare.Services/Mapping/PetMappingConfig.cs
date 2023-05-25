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
                .Map(dest => dest.PhotoUrl, src => src.PhotoUri);

            config.NewConfig<PostPetRequest, Pet>()
                .Map(dest => dest.PhotoUri, src => src.PhotoUrl);

            config.NewConfig<PutPetRequest, Pet>()
                .IgnoreNullValues(true);
        }
    }
}
