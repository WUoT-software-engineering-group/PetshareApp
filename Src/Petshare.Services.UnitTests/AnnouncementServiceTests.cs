using Petshare.CrossCutting.DTO.Announcement;
using Petshare.DataPersistence;
using Petshare.DataPersistence.Repositories;
using Petshare.Domain.Entities;
using Petshare.Domain.Repositories.Abstract;
using Petshare.Services.UnitTests.TestData;

namespace Petshare.Services.UnitTests
{
    public class AnnouncementServiceTests
    {
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

            var filteredAnnouncements = await announcementService.GetByFilters(filters);
            var filteredIds = filteredAnnouncements.Select(x => x.ID).ToList();

            Assert.True(filteredIds.SequenceEqual(expectedIds));
        }
    }
}
