using Petshare.Services.Abstract;

namespace Petshare.WebAPI.Configuration;

public class ConfigurationsManager : IServicesConfiguration
{
    private readonly AuthConfig _authConfig;
    private readonly MailConfig _mailConfig;

    public readonly IConfiguration Configuration;

    public ConfigurationsManager(IConfiguration configuration)
    {
        Configuration = configuration;
        DatabaseConnectionString = Configuration.GetValue<string>("SSDatabaseConnectionString")!;
        BlobStorageConnectionString = Configuration.GetValue<string>("SSBlobStorageConnectionString")!;
        _authConfig = Configuration.GetRequiredSection(AuthConfig.SectionName).Get<AuthConfig>()!;
        _mailConfig = new MailConfig
        {
            ApiKey = Configuration.GetValue<string>("SendGridAPIKey")!
        };
    }

    public string DatabaseConnectionString { get; }
    public string BlobStorageConnectionString { get; }
    public string BlobContainerName => "photos";
    public string AuthAuthority => _authConfig.Authority;
    public string AuthAudience => _authConfig.Audience;
    public string MailApiKey => _mailConfig.ApiKey;
    public string MailAddress => MailConfig.Email;
    public string MailName => MailConfig.Name;
}