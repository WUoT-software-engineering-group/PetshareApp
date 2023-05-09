using Petshare.CrossCutting.DTO.Pet;
using Petshare.CrossCutting.Utils;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;

namespace Petshare.Services.UnitTests
{
    public class PetServiceUnitTests
    {
        [Fact]
        public async void Create_ReturnsCreatedPetId()
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
            var userId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var updateParams = new PutPetRequest();

            var petToUpdate = updateParams.Adapt<Pet>();
            petToUpdate.ID = petId;
            petToUpdate.Shelter = new Shelter { ID = userId };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { petToUpdate }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var resultAdmin = await petService.Update(userId, "admin", petId, updateParams);
            var resultShelter = await petService.Update(userId, "shelter", petId, updateParams);

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
            var petId = Guid.NewGuid();
            var updateParams = new PutPetRequest();

            var petToUpdate = updateParams.Adapt<Pet>();
            petToUpdate.ID = petId;
            petToUpdate.Shelter = new Shelter { ID = Guid.NewGuid() };

            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet> { petToUpdate }.AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.Update(userId, "shelter", petId, updateParams);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.BadRequest());
        }
        
        [Fact]
        public async void Update_ReturnsBadRequestStatusIfNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var updateParams = new PutPetRequest();
        
            var petToUpdate = updateParams.Adapt<Pet>();
            petToUpdate.ID = petId;
            petToUpdate.Shelter = new Shelter { ID = userId };
        
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet>().AsEnumerable()));
        
            var petService = new PetService(repositoryWrapperMock.Object);
        
            // Act
            var result = await petService.Update(userId, "shelter", petId, updateParams);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.BadRequest());
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
            var result = await petService.GetByShelter(shelterId);
            var resultData = result.Data as List<PetResponse>;
        
            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Ok());
            Assert.NotNull(resultData);
            Assert.IsType<List<PetResponse>>(resultData);
            Assert.Equal(5, resultData.Count);
            Assert.True(resultData.All(x => x.ShelterID == shelterId));
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
            var resultData = result.Data as PetResponse;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Ok());
            Assert.NotNull(resultData);
            Assert.IsType<PetResponse>(resultData);
            Assert.Equal(petId, resultData.ID);
        }

        [Fact]
        public async void GetById_ReturnsNotFoundStatusIfPetNotFound()
        {
            // Arrange
             var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            repositoryWrapperMock.Setup(r => r.PetRepository.FindByCondition(It.IsAny<Expression<Func<Pet, bool>>>()))
                .Returns(Task.FromResult(new List<Pet>().AsEnumerable()));

            var petService = new PetService(repositoryWrapperMock.Object);

            // Act
            var result = await petService.GetById(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.True(result.StatusCode.NotFound());
            Assert.Null(result.Data);
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
            Assert.NotNull(result);
            Assert.True(result.StatusCode.Ok());
        }
    }
}
