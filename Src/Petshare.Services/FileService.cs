using Azure.Storage.Blobs;
using Petshare.Services.Abstract;

namespace Petshare.Services;

public class FileService : IFileService
{
    private readonly BlobContainerClient _blobContainerClient;

    public FileService(BlobContainerClient blobContainerClient)
    {
        _blobContainerClient = blobContainerClient;
    }
    
    public async Task<string> UploadFile(Stream fileStream, string fileName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
        return blobClient.Uri.AbsoluteUri;
    }
}