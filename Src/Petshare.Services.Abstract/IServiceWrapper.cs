namespace Petshare.Services.Abstract;

public interface IServiceWrapper
{
    IShelterService ShelterService { get; }

    IPetService PetService { get; }

    IAnnouncementService AnnouncementService { get; }

    IAdopterService AdopterService { get; }
    IFileService FileService { get; }
}