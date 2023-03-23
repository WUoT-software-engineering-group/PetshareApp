using Petshare.CrossCutting.DTO;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.Services.UnitTests
{
    public class PetServiceUnitTests
    {
        [Fact]
        public async void GetById_ReturnsPet()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var pet = new Pet { ID = petId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { pet }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.GetById(petId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PetDTO>(result);
            Assert.Equal(petId, result.ID);
        }

        [Fact]
        public async void GetById_ReturnsNullIfNotFound()
        {
            // Arrange
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet>().AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.GetById(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Create_ReturnsCreatedPet()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelter = new Shelter() { ID = shelterId };
            var petToCreate = new PetDTO { ID = Guid.NewGuid(), ShelterID = shelterId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.Create(It.IsAny<Pet>()))
                .Returns(Task.FromResult(petToCreate.Adapt<Pet>()));

            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelter }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Create(petToCreate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PetDTO>(result);
            Assert.Equal(petToCreate.ID, result.ID);
        }

        [Fact]
        public async void Update_ReturnsTrueIfUpdated()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var petToUpdate = new PetDTO { ID = petId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { petToUpdate.Adapt<Pet>() }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Update(petToUpdate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void Update_ReturnsFalseIfIdsDontMatch()
        {
            // Arrange
            var petToUpdate = new PetDTO { ID = Guid.NewGuid() };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Update(petToUpdate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void Update_ReturnsFalseIfNotFound()
        {
            // Arrange
            var petToUpdate = new PetDTO { ID = Guid.NewGuid() };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet>().AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Update(petToUpdate);

            // Assert
            Assert.False(result);
        }
    }
}
