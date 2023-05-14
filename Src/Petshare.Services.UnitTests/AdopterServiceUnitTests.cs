using Petshare.CrossCutting.DTO.Address;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.Enums;
using Petshare.CrossCutting.Utils;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.Services.UnitTests;

public class AdopterServiceUnitTests
{
    [Fact]
    public async void GetAll_ReturnsListOfAdopters()
    {
        // Arrange
        IEnumerable<Adopter> adopters = new List<Adopter>
        {
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() },
            new() { ID = Guid.NewGuid() }
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindAll()).Returns(Task.FromResult(adopters));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.GetAll();
        var resultData = result.Data as List<GetAdopterResponse>;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<List<GetAdopterResponse>>(resultData);
        Assert.Equal(5, resultData.Count);
    }

    [Fact]
    public async void GetById_ReturnsAdopter()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var adopter = new Adopter
        {
            ID = adopterId,
            UserName = "TestAdopter",
            PhoneNumber = "123456789",
            Email = "test@test.pl",
            Address = new Address
            {
                Street = "TestStreet",
                City = "TestCity",
                Province = "TestProvince",
                Country = "TestCountry",
                PostalCode = "12345"
            }
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter> { adopter }.AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.GetById(adopterId);
        var resultData = result.Data as GetAdopterResponse;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.IsType<GetAdopterResponse>(resultData);
        Assert.Equal(adopterId, resultData.ID);
        Assert.Equal(adopter.UserName, resultData.UserName);
        Assert.Equal(adopter.PhoneNumber, resultData.PhoneNumber);
        Assert.Equal(adopter.Email, resultData.Email);
        Assert.Equal(adopter.Address.Street, resultData.Address?.Street);
        Assert.Equal(adopter.Address.City, resultData.Address?.City);
        Assert.Equal(adopter.Address.Province, resultData.Address?.Province);
        Assert.Equal(adopter.Address.Country, resultData.Address?.Country);
        Assert.Equal(adopter.Address.PostalCode, resultData.Address?.PostalCode);
    }

    [Fact]
    public async void GetById_ReturnsNotFoundStatusIfAdopterNotFound()
    {
        // Arrange
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter>().AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.GetById(Guid.NewGuid());

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.NotFound());
        Assert.Null(result.Data);
    }

    [Fact]
    public async void Create_ReturnsCreatedAdopterID()
    {
        // Arrange
        var adopterToCreate = new PostAdopterRequest
        {
            UserName = "TestAdopter",
            PhoneNumber = "123456789",
            Email = "test@test.pl",
            Address = new AddressRequest
            {
                Street = "TestStreet",
                City = "TestCity",
                Province = "TestProvince",
                Country = "TestCountry",
                PostalCode = "12345"
            }
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.Create(It.IsAny<Adopter>()))
            .Returns(Task.FromResult(adopterToCreate.Adapt<Adopter>()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.Create(adopterToCreate);
        var resultData = result.Data as Guid?;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Created());
        Assert.NotNull(resultData);
        Assert.IsType<Guid>(resultData);
    }

    [Fact]
    public async void UpdateStatus_ReturnsOkStatusIfAdopterFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var adopterToUpdate = new PutAdopterRequest
        {
            Status = AdopterStatus.Deleted
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter> { new() { ID = adopterId } }.AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.UpdateStatus(adopterId, adopterToUpdate);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
    }

    [Fact]
    public async void UpdateStatus_ReturnsNotFoundStatusIfAdopterNotFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var adopterToUpdate = new PutAdopterRequest
        {
            Status = AdopterStatus.Deleted
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter>().AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.UpdateStatus(adopterId, adopterToUpdate);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.NotFound());
    }

    [Fact]
    public async void CheckIfAdopterIsVerified_ReturnsTrueIfVerificationFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();
        var verification = new ShelterAdopterVerification
        {
            AdopterID = adopterId,
            ShelterID = shelterId,
        };

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ShelterAdopterVerificationRepository.FindByCondition(It.IsAny<Expression<Func<ShelterAdopterVerification, bool>>>()))
            .Returns(Task.FromResult(new List<ShelterAdopterVerification> { verification }.AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.CheckIfAdopterIsVerified(adopterId, shelterId);
        var resultData = result.Data as bool?;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.True(resultData);
    }

    [Fact]
    public async void CheckIfAdopterIsVerified_ReturnsFalseIfVerificationNotFound()
    {
        // Arrange
        var adopterId = Guid.NewGuid();
        var shelterId = Guid.NewGuid();

        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.ShelterAdopterVerificationRepository.FindByCondition(It.IsAny<Expression<Func<ShelterAdopterVerification, bool>>>()))
            .Returns(Task.FromResult(new List<ShelterAdopterVerification>().AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.CheckIfAdopterIsVerified(adopterId, shelterId);
        var resultData = result.Data as bool?;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.StatusCode.Ok());
        Assert.NotNull(resultData);
        Assert.False(resultData);
    }
}