namespace Petshare.WebAPI.Configuration;

public class AuthConfig
{
    public const string SectionName = "Auth";

    public string Authority { get; set; } = default!;

    public string Audience { get; set; } = default!;
}