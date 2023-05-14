namespace Petshare.Services.Abstract;

public interface IServicesConfiguration
{
    public string DatabaseConnectionString { get; }
    public string BlobStorageConnectionString { get; }
    public string BlobContainerName { get; }
    public string MailApiKey { get; }
    public string MailAddress { get; }
    public string MailName { get; }
}