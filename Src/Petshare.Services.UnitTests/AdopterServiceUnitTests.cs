using Petshare.CrossCutting.DTO.Address;
using Petshare.CrossCutting.DTO.Adopter;
using Petshare.CrossCutting.Enums;
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

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<GetAdopterResponse>>(result);
        Assert.Equal(5, result.Count);
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

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GetAdopterResponse>(result);
        Assert.Equal(adopterId, result.ID);
        Assert.Equal(adopter.UserName, result.UserName);
        Assert.Equal(adopter.PhoneNumber, result.PhoneNumber);
        Assert.Equal(adopter.Email, result.Email);
        Assert.Equal(adopter.Address.Street, result.Address?.Street);
        Assert.Equal(adopter.Address.City, result.Address?.City);
        Assert.Equal(adopter.Address.Province, result.Address?.Province);
        Assert.Equal(adopter.Address.Country, result.Address?.Country);
        Assert.Equal(adopter.Address.PostalCode, result.Address?.PostalCode);
    }

    [Fact]
    public async void GetById_ReturnsNullIfNotFound()
    {
        // Arrange
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryWrapperMock.Setup(r => r.AdopterRepository.FindByCondition(It.IsAny<Expression<Func<Adopter, bool>>>()))
            .Returns(Task.FromResult(new List<Adopter>().AsEnumerable()));

        var adopterService = new AdopterService(repositoryWrapperMock.Object);

        // Act
        var result = await adopterService.GetById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
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

        // Assert
        Assert.NotNull(result);
        Assert.IsType<Guid>(result);
    }

    [Fact]
    public async void UpdateStatus_ReturnsTrueIfAdopterFound()
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
        Assert.True(result);
    }

    [Fact]
    public async void UpdateStatus_ReturnsFalseIfAdopterNotFound()
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
        Assert.False(result);
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

        // Assert
        Assert.True(result);
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

        // Assert
        Assert.False(result);
    }
}