using Petshare.CrossCutting.DTO;
using Petshare.CrossCutting.DTO.Address;
using Petshare.CrossCutting.DTO.Shelter;
using Petshare.CrossCutting.Utils;
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
            var pageSize = 2;
            var result = await shelterService.GetAll( new PagingRequest{ PageNumber = 1, PageCount = pageSize });
            var resultData = result.Data as PagedShelterResponse;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Ok());
            Assert.NotNull(resultData);
            Assert.IsType<List<ShelterResponse>>(resultData.Shelters);
            Assert.Equal(pageSize, resultData.Shelters.Count);
            Assert.Equal(5, resultData.Count);
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
            var resultData = result.Data as ShelterResponse;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Ok());
            Assert.NotNull(resultData);
            Assert.IsType<ShelterResponse>(resultData);
            Assert.Equal(shelter.ID, resultData.ID);
            Assert.Equal(shelter.UserName, resultData.UserName);
            Assert.Equal(shelter.PhoneNumber, resultData.PhoneNumber);
            Assert.Equal(shelter.FullShelterName, resultData.FullShelterName);
            Assert.Equal(shelter.Address?.Street, resultData.Address.Street);
            Assert.Equal(shelter.Address?.City, resultData.Address.City);
            Assert.Equal(shelter.Address?.Province, resultData.Address.Province);
            Assert.Equal(shelter.Address?.Country, resultData.Address.Country);
            Assert.Equal(shelter.Address?.PostalCode, resultData.Address.PostalCode);
        }

        [Fact]
        public async void GetById_ReturnsNotFoundStatusIfShelterNotFound()
        {
            // Arrange
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter>().AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.GetById(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.NotFound());
            Assert.Null(result.Data);
        }

        [Fact]
        public async void Create_ReturnsCreatedShelterId()
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
            var resultData = result.Data as Guid?;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Created());
            Assert.NotNull(resultData);
            Assert.IsType<Guid>(resultData);
        }

        [Fact]
        public async void Update_ReturnsOkStatusIfUpdated()
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
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Ok());
        }

        [Fact]
        public async void Update_ReturnsBadRequestStatusIfNotFound()
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
            Assert.NotNull(result);
            Assert.True(result.StatusCode.BadRequest());
        }
    }
}