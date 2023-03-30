using Petshare.Services.Abstract;

namespace Petshare.WebAPI.Configuration;

public class ConfigurationsManager : IServicesConfiguration
{
    private readonly AuthConfig _authConfig;

    public readonly IConfiguration Configuration;

    public ConfigurationsManager(IConfiguration configuration)
    {
        Configuration = configuration;
        DatabaseConnectionString = Configuration.GetValue<string>("SSDatabaseConnectionString")!;
        _authConfig = Configuration.GetRequiredSection(AuthConfig.SectionName).Get<AuthConfig>()!;
    }

    public string DatabaseConnectionString { get; }

    public string AuthAuthority => _authConfig.Authority;

    public string AuthAudience => _authConfig.Audience;
}