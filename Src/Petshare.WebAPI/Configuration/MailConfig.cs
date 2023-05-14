namespace Petshare.WebAPI.Configuration;

public class MailConfig
{
    public string ApiKey { get; init; } = null!;

    public static string Email => "petshare.io@gmail.com";

    public static string Name => "PetShare";
}