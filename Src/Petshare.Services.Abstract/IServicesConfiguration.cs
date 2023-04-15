namespace Petshare.Services.Abstract;

public interface IServicesConfiguration
{
    public string DatabaseConnectionString { get; }
    public string BlobStorageConnectionString { get; }
    public string BlobContainerName { get; }
}