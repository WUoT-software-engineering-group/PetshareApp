using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Enums;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.Services.UnitTests;

public class AnnouncementServiceUnitTests
{
    [Fact]
    public async Task Create_ReturnsCreatedAnnouncementIfPetIdGiven()
    {
        // Arrange
        var shelterMock = new Shelter
        {
            ID = Guid.NewGuid()
        };
        var petMock = new Pet
        {
            ID = Guid.NewGuid(),
            Shelter = shelterMock
        };
        var announcementToCreate = new PostAnnouncementRequest
        {
            Title = "Test Announcement",
            Description = "Test Announcement Description",
            PetId = petMock.ID
        };
        var announcementMock = announcementToCreate.Adapt<Announcement>();
        announcementMock.Pet = petMock;

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.Create(It.IsAny<Announcement>()))
            .Returns(Task.FromResult(announcementMock));

        repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
            .Returns(Task.FromResult(new List<Pet> { petMock }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Create(shelterMock.ID, announcementToCreate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(announcementToCreate.Title, result.Title);
        Assert.Equal(announcementToCreate.Description, result.Description);
        Assert.Equal(petMock.ID, result.Pet.ID);
        Assert.Equal(shelterMock.ID, result.Pet.ShelterID);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAnnouncementIfPetGiven()
    {
        // Arrange
        var shelterMock = new Shelter
        {
            ID = Guid.NewGuid()
        };
        var petMock = new Pet
        {
            ID = Guid.NewGuid(),
            Shelter = shelterMock,
            Name = "Test",
            Description = "Test"
        };
        var announcementToCreate = new PostAnnouncementRequest
        {
            Title = "Test Announcement",
            Description = "Test Announcement Description",
            Pet = petMock.Adapt<PostPetRequest>()
        };

        var announcementMock = announcementToCreate.Adapt<Announcement>();
        announcementMock.Pet = petMock;

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.Create(It.IsAny<Announcement>()))
            .Returns(Task.FromResult(announcementMock));

        repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
            .Returns(Task.FromResult(new List<Shelter> { shelterMock }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Create(shelterMock.ID, announcementToCreate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(announcementToCreate.Title, result.Title);
        Assert.Equal(announcementToCreate.Description, result.Description);
        Assert.Equal(petMock.ID, result.Pet.ID);
        Assert.Equal(shelterMock.ID, result.Pet.ShelterID);
        Assert.Equal(petMock.Name, result.Pet.Name);
        Assert.Equal(petMock.Description, result.Pet.Description);
    }

    [Fact]
    public async Task Create_ReturnsNullIfPetIdGivenAndPetNotFound()
    {
        // Arrange
        var shelterMock = new Shelter
        {
            ID = Guid.NewGuid()
        };
        var announcementToCreate = new PostAnnouncementRequest
        {
            Title = "Test Announcement",
            Description = "Test Announcement Description",
            PetId = Guid.NewGuid()
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
            .Returns(Task.FromResult(new List<Pet>().AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Create(shelterMock.ID, announcementToCreate);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnsNullIfPetIdGivenAndShelterIdsDontMatch()
    {
        // Arrange
        var shelterMock = new Shelter
        {
            ID = Guid.NewGuid()
        };
        var petMock = new Pet
        {
            ID = Guid.NewGuid(),
            Shelter = shelterMock
        };
        var announcementToCreate = new PostAnnouncementRequest
        {
            Title = "Test Announcement",
            Description = "Test Announcement Description",
            PetId = petMock.ID
        };

        var announcementMock = announcementToCreate.Adapt<Announcement>();
        announcementMock.Pet = petMock;

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
            .Returns(Task.FromResult(new List<Pet> { petMock }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Create(Guid.NewGuid(), announcementToCreate);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ReturnsNullIfPetGivenAndShelterNotFound()
    {
        // Arrange
        var announcementToCreate = new PostAnnouncementRequest
        {
            Title = "Test Announcement",
            Description = "Test Announcement Description",
            Pet = new PostPetRequest
            {
                Name = "Test",
                Description = "Test"
            }
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
            .Returns(Task.FromResult(new List<Shelter>().AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Create(Guid.NewGuid(), announcementToCreate);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async void Update_ReturnsTrueIfUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        var announcementToUpdate = new PutAnnouncementRequest
        {
            Title = "TestTitle",
            Description = "TestDescription",
            Status = AnnouncementStatus.Deleted
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement> { announcementToUpdate.Adapt<Announcement>() }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Update(userId, announcementId, announcementToUpdate);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async void Update_ReturnsFalseIfNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        var announcementToUpdate = new PutAnnouncementRequest
        {
            Title = "TestTitle",
            Description = "TestDescription",
            Status = AnnouncementStatus.Deleted
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement>().AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Update(userId, announcementId, announcementToUpdate);

        // Assert
        Assert.False(result);
    }
}