using Petshare.CrossCutting.DTO.Address;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.Services.UnitTests
{
    public class ShelterServiceUnitTests
    {
        [Fact]
        public async void GetAll_ReturnsListOfShelters()
        {
            // Arrange
            IEnumerable<Shelter> shelters = new List<Shelter>
            {
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
                new() { ID = Guid.NewGuid() },
            };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindAll()).Returns(Task.FromResult(shelters));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ShelterResponse>>(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async void GetById_ReturnsShelter()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelter = new Shelter
            {
                ID = shelterId,
                UserName = "TestShelter",
                PhoneNumber = "123456789",
                FullShelterName = "TestShelter",
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
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelter }.AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.GetById(shelterId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShelterResponse>(result);
            Assert.Equal(shelter.ID, result.ID);
            Assert.Equal(shelter.UserName, result.UserName);
            Assert.Equal(shelter.PhoneNumber, result.PhoneNumber);
            Assert.Equal(shelter.FullShelterName, result.FullShelterName);
            Assert.Equal(shelter.Address.Street, result.Address.Street);
            Assert.Equal(shelter.Address.City, result.Address.City);
            Assert.Equal(shelter.Address.Province, result.Address.Province);
            Assert.Equal(shelter.Address.Country, result.Address.Country);
            Assert.Equal(shelter.Address.PostalCode, result.Address.PostalCode);
        }

        [Fact]
        public async void GetById_ReturnsNullIfNotFound()
        {
            // Arrange
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter>().AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ReturnsCreatedShelter()
        {
            // Arrange
            var shelterToCreate = new PostShelterRequest
            {
                UserName = "TestShelter",
                PhoneNumber = "123456789",
                FullShelterName = "TestShelter",
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
            repositoryWrapperMock.Setup(r => r.ShelterRepository.Create(It.IsAny<Shelter>()))
                .Returns(Task.FromResult(shelterToCreate.Adapt<Shelter>()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Create(shelterToCreate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShelterResponse>(result);
            Assert.Equal(shelterToCreate.UserName, result.UserName);
            Assert.Equal(shelterToCreate.PhoneNumber, result.PhoneNumber);
            Assert.Equal(shelterToCreate.FullShelterName, result.FullShelterName);
            Assert.Equal(shelterToCreate.Address.Street, result.Address.Street);
            Assert.Equal(shelterToCreate.Address.City, result.Address.City);
            Assert.Equal(shelterToCreate.Address.Province, result.Address.Province);
            Assert.Equal(shelterToCreate.Address.Country, result.Address.Country);
            Assert.Equal(shelterToCreate.Address.PostalCode, result.Address.PostalCode);
        }

        [Fact]
        public async void Update_ReturnsTrueIfUpdated()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelterToUpdate = new PutShelterRequest() { IsAuthorized = true};

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelterToUpdate.Adapt<Shelter>() }.AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Update(shelterId, shelterToUpdate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void Update_ReturnsFalseIfNotFound()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelterToUpdate = new PutShelterRequest { IsAuthorized = true };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter>().AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Update(shelterId, shelterToUpdate);

            // Assert
            Assert.False(result);
        }
    }
}