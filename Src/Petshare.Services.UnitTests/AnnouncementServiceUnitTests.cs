using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.Enums;
using Petshare.CrossCutting.Utils;
using Petshare.DataPersistence;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.UnitTests.TestData;
using Petshare.DataPersistence.Repositories;

namespace Petshare.Services.UnitTests;

public class AnnouncementServiceUnitTests
{
    [Fact]
    public async Task Create_ReturnsCreatedAnnouncement()
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
        var resultData = result.Data as Guid?;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Created());
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData);
    }

    [Fact]
    public async Task Create_ReturnsBadRequestStatusIfPetNotFound()
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
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }

    [Fact]
    public async Task Create_ReturnsBadRequestStatusIfPetShelterIdAndGivenShelterIdDontMatch()
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
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }

    [Fact]
    public async Task Create_ReturnsBadRequestStatusIfGivenAndShelterNotFound()
    {
        // Arrange
        var announcementToCreate = new PostAnnouncementRequest
        {
            Title = "Test Announcement",
            Description = "Test Announcement Description",
            PetId = Guid.NewGuid(),
        };
        var petMock = new Pet
        {
            ID = Guid.NewGuid(),
            Shelter = new()
            {
                ID = Guid.NewGuid()
            }
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
            .Returns(Task.FromResult(new List<Pet> { petMock }.AsEnumerable()));
        repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
            .Returns(Task.FromResult(new List<Shelter>().AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Create(Guid.NewGuid(), announcementToCreate);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }

    [Fact]
    public async void Update_ReturnsOkStatusIfUpdated()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        var announcementToUpdate = new Announcement { Author = new Shelter { ID = userId } };
        var announcementToUpdateRequest = new PutAnnouncementRequest
        {
            Title = "TestTitle",
            Description = "TestDescription",
            Status = AnnouncementStatus.Deleted
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement> { announcementToUpdate }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var resultAdmin = await announcementService.Update(userId, "admin", announcementId, announcementToUpdateRequest);
        var resultShelter = await announcementService.Update(userId, "shelter", announcementId, announcementToUpdateRequest);

        // Assert
        Assert.NotNull(resultAdmin);
        Assert.NotNull(resultShelter);
        Assert.True(resultAdmin.StatusCode.Ok());
        Assert.True(resultShelter.StatusCode.Ok());
    }
    
    [Fact]
    public async void Update_ReturnsBadRequestStatusIfShelterIsNotAuthor()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        var announcementToUpdate = new Announcement { Author = new Shelter { ID = Guid.NewGuid() } };
        var announcementToUpdateRequest = new PutAnnouncementRequest
        {
            Title = "TestTitle",
            Description = "TestDescription",
            Status = AnnouncementStatus.Deleted
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement> { announcementToUpdate }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.Update(userId, "shelter", announcementId, announcementToUpdateRequest);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }

    [Fact]
    public async void Update_ReturnsBadRequestStatusIfNotFound()
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
        var result = await announcementService.Update(userId, "shelter", announcementId, announcementToUpdate);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }

    [Fact]
    public async void GetById_ReturnsAnnouncement()
    {
        // Arrange
        var announcementId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var announcement = new Announcement
        {
            ID = announcementId,
            Author = new()
            {
                ID = shelterId,
            },
            ClosingDate = DateTime.Now.AddDays(7),
            CreationDate = DateTime.Now,
            Description = "SomeDesc",
            LastUpdateDate = DateTime.Now,
            Status = AnnouncementStatus.Open,
            Pet = new()
            {
                ID = Guid.NewGuid(),
                Shelter = new()
                {
                    ID = shelterId
                }
            },
            Title = "SomeTitle"
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement> { announcement }.AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.GetById(announcementId);
        var resultData = result.Data as AnnouncementResponse;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<AnnouncementResponse>(resultData);
        Assert.Equal(announcement.ID, resultData.ID);
        Assert.Equal(announcement.ClosingDate, resultData.ClosingDate);
        Assert.Equal(announcement.CreationDate, resultData.CreationDate);
        Assert.Equal(announcement.Title, resultData.Title);
        Assert.Equal(announcement.Description, resultData.Description);
        Assert.Equal(announcement.Pet.ID, resultData.Pet.ID);
        Assert.Equal(announcement.LastUpdateDate, resultData.LastUpdateDate);
        Assert.Equal(announcement.Status, resultData.Status);
    }

    [Fact]
    public async void GetById_ReturnsNotFoundStatusIfAnnouncementNotFound()
    {
        // Arrange
        var announcementId = Guid.NewGuid();

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement>().AsEnumerable()));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        // Act
        var result = await announcementService.GetById(announcementId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.NotFound());
        Assert.Null(result.Data);
    }

    [Theory]
    [ClassData(typeof(GetByFiltersTestData))]
    public async void GetByFilters_ReturnsFilteredAnnouncements(GetAnnouncementsRequest filters, List<Guid> expectedIds)
    {
        // Arrange
        var dbMock = new Mock<IPetshareDbContext>();
        dbMock.Setup(db => db.Set<Announcement>())
            .Returns(GetByFiltersTestData.RepositoryData.GetMockDbSet().Object);

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository)
            .Returns(new Repository<Announcement>(dbMock.Object));

        var announcementService = new AnnouncementService(repositoryWrapperMock.Object);

        var result = await announcementService.GetByFilters(filters, null);
        var filteredAnnouncements = result.Data as List<AnnouncementResponse>;
        var filteredIds = filteredAnnouncements?.Select(x => x.ID).ToList();

        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(filteredAnnouncements);
        Assert.True(filteredIds?.SequenceEqual(expectedIds));
    }
}