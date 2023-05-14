using Mapster;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.Domain.Entities;

namespace Petshare.Services.Mapping;

public class AnnouncementMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PutAnnouncementRequest, Announcement>()
            .Map(dest => dest.LastUpdateDate, src => DateTime.Now)
            .IgnoreNullValues(true);
    }
}