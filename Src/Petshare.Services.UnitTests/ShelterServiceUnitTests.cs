using Petshare.CrossCutting.DTO;
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
            Assert.IsType<List<ShelterDTO>>(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async void GetById_ReturnsShelter()      
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelter = new Shelter { ID = shelterId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelter}.AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.GetById(shelterId.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShelterDTO>(result);
            Assert.Equal(shelterId, result.ID);
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
            var result = await shelterService.GetById(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ReturnsCreatedShelter()
        {
            // Arrange
            var shelterToCreate = new ShelterDTO { ID = Guid.NewGuid() };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.Create(It.IsAny<Shelter>()))
                .Returns(Task.FromResult(shelterToCreate.Adapt<Shelter>()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Create(shelterToCreate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ShelterDTO>(result);
            Assert.Equal(shelterToCreate.ID, result.ID);
        }

        [Fact]
        public async void Update_ReturnsTrueIfUpdated()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelterToUpdate = new ShelterDTO { ID = shelterId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelterToUpdate.Adapt<Shelter>() }.AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Update(shelterId.ToString(), shelterToUpdate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void Update_ReturnsFalseIfIdsDontMatch()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelterToUpdate = new ShelterDTO { ID = Guid.NewGuid() };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelterToUpdate.Adapt<Shelter>() }.AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Update(shelterId.ToString(), shelterToUpdate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void Update_ReturnsFalseIfNotFound()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelterToUpdate = new ShelterDTO { ID = shelterId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter>().AsEnumerable()));

            var shelterService = new ShelterService(repositoryWrapperMock.Object);

            // Act
            var result = await shelterService.Update(shelterId.ToString(), shelterToUpdate);

            // Assert
            Assert.False(result);
        }
    }
}