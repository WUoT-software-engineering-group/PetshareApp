using System.Drawing.Printing;
using System.Net;
using Petshare.CrossCutting.DTO.Announcement;
using Petshare.CrossCutting.DTO.Applications;
using Petshare.CrossCutting.Enums;
using Petshare.CrossCutting.Utils;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.Abstract;
using SendGrid;

namespace Petshare.Services.UnitTests;

public class ApplicationsServiceUnitTests
{
    [Fact]
    public async void Create_ReturnsCreatedApplicationIDifAnnouncementAndAdopterWereFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter> { new() { ID = adopterId } }.AsEnumerable()));
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement> { new() { ID = announcementId } }.AsEnumerable()));
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.Create(It.IsAny<Application>()))
            .Returns(Task.FromResult(new Application()));

        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var result = await applicationsService.Create(announcementId, adopterId);
        var resultData = result.Data as Guid?;
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Created());
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData);
    }
    
    [Fact]
    public async void Create_ReturnsBadRequestIfAnnouncementWasNotFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter> { new() { ID = adopterId } }.AsEnumerable()));
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement>().AsEnumerable()));

        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var result = await applicationsService.Create(announcementId, adopterId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }
    
    [Fact]
    public async void Create_ReturnsBadRequestIfAdopterWasNotFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var announcementId = Guid.NewGuid();
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter>().AsEnumerable()));
        repositoryWrapperMock.Setup(r => r.AnnouncementRepository.FindByCondition(It.IsAny<Expression<Func<Announcement, bool>>>()))
            .Returns(Task.FromResult(new List<Announcement> { new() { ID = announcementId } }.AsEnumerable()));

        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var result = await applicationsService.Create(announcementId, adopterId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }

    [Fact]
    public async void GetAll_ReturnsListOfAllApplicationsWhenRoleIsAdmin()
    {
        // Arrange
        IEnumerable<Application> applications = new List<Application>()
        {
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
        };
        
        var adminId = Guid.NewGuid();
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindAll())
            .Returns(Task.FromResult(applications));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var pageSize = 2;
        var result = await applicationsService.GetAll("admin", adminId, 1, pageSize);
        var resultData = result.Data as PagedApplicationResponse;
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<List<ApplicationResponse>>(resultData.Applications);
        Assert.Equal(pageSize, resultData.Applications.Count);
        Assert.Equal(5, resultData.Count);
    }
    
    [Fact]
    public async void GetAll_ReturnsListOfAdopterApplicationsWhenRoleIsAdopter()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var notAdopterId = Guid.NewGuid();

        IEnumerable<Application> applications = new List<Application>()
        {
            new() { Adopter = new() { ID = adopterId } },
            new() { Adopter = new() { ID = adopterId } },
            new() { Adopter = new() { ID = adopterId } },
            new() { Adopter = new() { ID = adopterId } },
            new() { Adopter = new() { ID = notAdopterId } },
        };
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindByCondition(It.IsAny<Expression<Func<Application, bool>>>()))
            .Returns(Task.FromResult(applications.Where(a => a.Adopter.ID == adopterId)));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);
     
        // Act
        var pageSize = 2;
        var result = await applicationsService.GetAll("adopter", adopterId, 1, pageSize);
        var resultData = result.Data as PagedApplicationResponse;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<List<ApplicationResponse>>(resultData.Applications);
        Assert.Equal(pageSize, resultData.Applications.Count);
        Assert.Equal(4, resultData.Count);
    }
    
    [Fact]
    public async void GetAll_ReturnsListOfApplicationsWhoseAuthorIsShelterWhenRoleIsShelter()
    {
        // Arrange
        var shelterId = Guid.NewGuid();
        var notShelterId = Guid.NewGuid();

        IEnumerable<Application> applications = new List<Application>()
        {
            new() { Announcement = new() { Author = new() { ID = shelterId } } },
            new() { Announcement = new() { Author = new() { ID = shelterId } } },
            new() { Announcement = new() { Author = new() { ID = shelterId } } },
            new() { Announcement = new() { Author = new() { ID = shelterId } } },
            new() { Announcement = new() { Author = new() { ID = notShelterId } } },
        };
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindByCondition(It.IsAny<Expression<Func<Application, bool>>>()))
            .Returns(Task.FromResult(applications.Where(a => a.Announcement.Author.ID == shelterId)));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var pageSize = 2;
        var result = await applicationsService.GetAll("shelter", shelterId, 1, pageSize);
        var resultData = result.Data as PagedApplicationResponse;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<List<ApplicationResponse>>(resultData.Applications);
        Assert.Equal(pageSize, resultData.Applications.Count);
        Assert.Equal(4, resultData.Count);
    }
    
    [Fact]
    public async void GetAll_ThrowsNotImplementedExceptionWhenRoleIsUnknown()
    {
        // Arrange
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);
        
        // Assert
        await Assert.ThrowsAsync<NotImplementedException>(async () => await applicationsService.GetAll("unknown role", Guid.NewGuid(), 1, 2));
    }

    [Fact]
    public async void GetByAnnouncement_ReturnsListOfAllAnnouncementApplications()
    {
        // Arrange
        var announcementId = Guid.NewGuid();
        var notAnnouncementId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();

        IEnumerable<Application> applications = new List<Application>()
        {
            new() { Announcement = new() { ID = announcementId } },
            new() { Announcement = new() { ID = announcementId } },
            new() { Announcement = new() { ID = announcementId } },
            new() { Announcement = new() { ID = announcementId } },
            new() { Announcement = new() { ID = notAnnouncementId } },
        };
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindByCondition(It.IsAny<Expression<Func<Application, bool>>>()))
            .Returns(Task.FromResult(applications.Where(a => a.Announcement.ID == announcementId)));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        serviceWrapperMock.Setup(s => s.AnnouncementService.GetById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(ServiceResponse.Ok(
                new AnnouncementResponse()
                {
                    ID = announcementId, 
                    Pet = new() { Shelter = new() {ID = shelterId} }
                })));
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var pageSize = 2;
        var result = await applicationsService.GetByAnnouncement(announcementId, shelterId, 1, pageSize);
        var resultData = result.Data as PagedApplicationResponse;
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<List<ApplicationResponse>>(resultData.Applications);
        Assert.Equal(pageSize, resultData.Applications.Count);
        Assert.Equal(4, resultData.Count);
    }
    
    [Fact]
    public async void GetByAnnouncement_ReturnsBadRequestIfAnnouncementWasNotFound()
    {
        // Arrange
        var announcementId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        serviceWrapperMock.Setup(s => s.AnnouncementService.GetById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(ServiceResponse.NotFound()));
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);
     
        // Act
        var result = await applicationsService.GetByAnnouncement(announcementId, shelterId, 1, 2);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }
    
    [Fact]
    public async void GetByAnnouncement_ReturnsBadRequestIfShelterIDDoesntMatch()
    {
        // Arrange
        var announcementId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var notShelterId = Guid.NewGuid();

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        serviceWrapperMock. Setup(s => s.AnnouncementService.GetById(It.IsAny<Guid>()))
            .Returns(Task.FromResult(ServiceResponse.Ok(
                new AnnouncementResponse()
                {
                    ID = announcementId, 
                    Pet = new() { Shelter = new() {ID = notShelterId} }
                })));
        
        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);
     
        // Act
        var result = await applicationsService.GetByAnnouncement(announcementId, shelterId, 1, 2);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }
    
    [Fact]
    public async void UpdateStatus_OkStatusWhenUpdateWasSuccessful()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var oldStatus = ApplicationStatus.Created;
        var newStatus = ApplicationStatus.Accepted;

        var userName = "userName";
        var userEmail = "userEmail";
        var petName = "petName";
        var petBreed = "petBreed";
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindByCondition(It.IsAny<Expression<Func<Application, bool>>>()))
            .Returns(Task.FromResult(new List<Application>()
            {
                new()
                {
                    ID = applicationId,
                    Adopter = new() { UserName = userName, Email = userEmail },
                    Announcement = new()
                    {
                        Pet = new Pet() { Name = petName, Breed = petBreed },
                        Author = new() { ID = shelterId }
                    },
                    Status = oldStatus,
                    LastUpdateDate = DateTime.Now
                }
            }.AsEnumerable()));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();
        serviceWrapperMock.Setup(s => s.MailService.SendApplicationDecisionMail(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ApplicationStatus>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(new Response(HttpStatusCode.OK, new MultipartContent(), new HttpResponseMessage().Headers))!);

        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var result = await applicationsService.UpdateStatus(applicationId, newStatus, shelterId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
    }

    [Fact]
    public async void UpdateStatus_ReturnsBadRequestIfApplicationWasNotFound()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var newStatus = ApplicationStatus.Created;
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindByCondition(It.IsAny<Expression<Func<Application, bool>>>()))
            .Returns(Task.FromResult(new List<Application>().AsEnumerable()));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();

        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var result = await applicationsService.UpdateStatus(applicationId, newStatus, shelterId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }
    
    [Fact]
    public async void UpdateStatus_ReturnsBadRequestIfAuthorIdDoesntMatch()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var notShelterId = Guid.NewGuid();
        var newStatus = ApplicationStatus.Created;
        
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ApplicationsRepository.FindByCondition(It.IsAny<Expression<Func<Application, bool>>>()))
            .Returns(Task.FromResult(new List<Application>()
            {
                new()
                {
                    ID = applicationId,
                    Announcement = new() { Author = new() { ID = notShelterId } }
                }
            }.AsEnumerable()));
        
        var serviceWrapperMock = new Mock<IServiceWrapper>();

        var applicationsService = new ApplicationsService(repositoryWrapperMock.Object, serviceWrapperMock.Object);

        // Act
        var result = await applicationsService.UpdateStatus(applicationId, newStatus, shelterId);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.BadRequest());
    }
}