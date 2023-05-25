using Mapster;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.Domain.Entities;

namespace Petshare.Services.Mapping;

public class ApplicationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Application, ApplicationResponse>()
            .Map(dest => dest.AnnouncementId, src => src.Announcement.ID)
            .Map(dest => dest.ApplicationStatus, src => src.Status);
    }
}