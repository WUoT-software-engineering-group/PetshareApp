
namespace Petshare.Services.Abstract;

public interface IFileService
{
   public Task<string> UploadFile(Stream fileStream, string fileName);
}