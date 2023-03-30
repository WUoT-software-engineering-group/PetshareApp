using Petshare.Services.Abstract;

namespace Petshare.WebAPI.Configuration;

public class ConfigurationsManager : IServicesConfiguration
{
    private readonly DatabaseConfig _databaseConfig;
    private readonly AuthConfig _authConfig;

    public readonly IConfiguration Configuration;

    public ConfigurationsManager(IConfiguration configuration)
    {
        Configuration = configuration;
        _databaseConfig = Configuration.GetRequiredSection(DatabaseConfig.SectionName).Get<DatabaseConfig>()!;
        _authConfig = Configuration.GetRequiredSection(AuthConfig.SectionName).Get<AuthConfig>()!;
    }

    public string DatabaseConnectionString =>
        $"Server={_databaseConfig.Server};Initial Catalog={_databaseConfig.DbName};" +
        "Persist Security Info=False;" +
        $"User ID={_databaseConfig.Login};Password={_databaseConfig.Password};" +
        "MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=15;";

    public string AuthAuthority => _authConfig.Authority;

    public string AuthAudience => _authConfig.Audience;
}