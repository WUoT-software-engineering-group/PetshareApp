using Petshare.CrossCutting.DTO.Pet;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.Services.UnitTests
{
    public class PetServiceUnitTests
    {
        [Fact]
        public async void Create_ReturnsCreatedPet()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var shelter = new Shelter() { ID = shelterId };
            var petToCreate = new PostPetRequest();

            var petId = Guid.NewGuid();
            var createdPet = petToCreate.Adapt<Pet>();
            createdPet.ID = petId;

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.Create(It.IsAny<Pet>()))
                .Returns(Task.FromResult(createdPet));

            repositoryWrapperMock.Setup(r => r.ShelterRepository.FindByCondition(It.IsAny<Expression<Func<Shelter, bool>>>()))
                .Returns(Task.FromResult(new List<Shelter> { shelter }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Create(shelterId, petToCreate);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<PetResponse>(result);
            Assert.Equal(petId, result.ID);
        }

        [Fact]
        public async void Update_ReturnsTrueIfUpdated()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var updateParams = new PutPetRequest();

            var petToUpdate = updateParams.Adapt<Pet>();
            petToUpdate.ID = petId;

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { petToUpdate }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Update(petId, updateParams);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async void Update_ReturnsFalseIfNotFound()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var updateParams = new PutPetRequest();

            var petToUpdate = updateParams.Adapt<Pet>();
            petToUpdate.ID = petId;

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet>().AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Update(petId, updateParams);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async void GetByShelter_ReturnsListOfPetsFromThatShelter()
        {
            // Arrange
            var shelterId = Guid.NewGuid();
            var properShelter = new Shelter { ID = shelterId };

            IEnumerable<Pet> pets = new List<Pet>
            {
                new() { ID = Guid.NewGuid(), Shelter = properShelter },
                new() { ID = Guid.NewGuid(), Shelter = properShelter },
                new() { ID = Guid.NewGuid(), Shelter = properShelter },
                new() { ID = Guid.NewGuid(), Shelter = properShelter },
                new() { ID = Guid.NewGuid(), Shelter = properShelter },
            };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(pets));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            //TODO: uncomment when GetByShelter method is completed
            var result = await petService.GetByShelter(/*shelterId*/);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PetResponse>>(result);
            //TODO: uncomment when GetByShelter method is completed
            //Assert.Equal(5, result.Count);
            //Assert.True(result.All(x => x.ShelterID == shelterId));
        }

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
            Assert.IsType<PetResponse>(result);
            Assert.Equal(petId, result.ID);
        }

        [Fact]
        public async void GetById_ReturnsNullIfNotFound()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var pet = new Pet { ID = petId };
            
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
        public async void UpdatePhotoUri_UpdatesPetPhoto()
        {
            // Arrange
            var petId = Guid.NewGuid();
            var shelterId = Guid.NewGuid();
            var pet = new Pet { ID = petId, Shelter = new Shelter {ID = shelterId}};

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { pet }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);
            
            // Act
            var result = await petService.UpdatePhotoUri(petId, shelterId, "testUri");
            
            // Assert
            Assert.True(result);
        }
    }
}
