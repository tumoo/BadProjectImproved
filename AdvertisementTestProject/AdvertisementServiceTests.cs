using Moq;
using BadProject.Cache.Contracts;
using BadProject.Services.Contracts;
using Adv;
using ThirdParty;

namespace AdvertisementTestProject
{
    public class AdvertisementServiceTests
    {
        [Fact]
        public void GetAdvertisement_CacheHit_ReturnsAdvertisement()
        {
            // Arrange
            var cacheProviderMock = new Mock<ICacheProvider>();
            var noSqlProviderMock = new Mock<INoSqlAdvProviderService>();
            var sqlProviderMock = new Mock<ISQLAdvProviderService>();

            var advertisementService = new AdvertisementService(noSqlProviderMock.Object, sqlProviderMock.Object, cacheProviderMock.Object);

            var advertisementId = "100";
            var cachedAdvertisement = new Advertisement { WebId = advertisementId, Description = "Test1", Name = "CampaignAd" };

            cacheProviderMock.Setup(c => c.GetAdvertisementFromCache(advertisementId)).Returns(cachedAdvertisement);

            // Act
            var result = advertisementService.GetAdvertisement(advertisementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cachedAdvertisement, result);

            // Verify that other methods were not called
            noSqlProviderMock.Verify(n => n.GetAdv(It.IsAny<string>(), It.IsAny<Queue<DateTime>>()), Times.Never);
            sqlProviderMock.Verify(s => s.GetAdv(It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public void GetAdvertisement_CacheMiss_NoSqlProviderSuccess_ReturnsAdvertisement()
        {
            // Arrange
            var cacheProviderMock = new Mock<ICacheProvider>();
            var noSqlProviderMock = new Mock<INoSqlAdvProviderService>();
            var sqlProviderMock = new Mock<ISQLAdvProviderService>();

            var advertisementService =  new AdvertisementService(noSqlProviderMock.Object, sqlProviderMock.Object, cacheProviderMock.Object);

            var advertisementId = "333";
            Advertisement noSqlAdvertisement = new Advertisement { WebId = advertisementId, Description = "TestAdvert2", Name = "Campaign Ad" };

            cacheProviderMock.Setup(c => c.GetAdvertisementFromCache(advertisementId)).Returns((Advertisement)null);
            noSqlProviderMock.Setup(n => n.GetAdv(advertisementId, It.IsAny<Queue<DateTime>>())).Returns(noSqlAdvertisement);

            // Act
            var result = advertisementService.GetAdvertisement(advertisementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(noSqlAdvertisement, result);

            // Verify that the NoSqlProvider method was called
            noSqlProviderMock.Verify(n => n.GetAdv(advertisementId, It.IsAny<Queue<DateTime>>()), Times.Once);

            // Verify that the SQLProvider method was not called
            sqlProviderMock.Verify(s => s.GetAdv(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public void GetAdvertisement_CacheMiss_NoSqlProviderFailure_SQLProviderSuccess_ReturnsAdvertisement()
        {
            // Arrange
            var cacheProviderMock = new Mock<ICacheProvider>();
            var noSqlProviderMock = new Mock<INoSqlAdvProviderService>();
            var sqlProviderMock = new Mock<ISQLAdvProviderService>();

            var advertisementService = new AdvertisementService(noSqlProviderMock.Object, sqlProviderMock.Object, cacheProviderMock.Object);

            var advertisementId = "555";
            Advertisement sqlAdvertisement = new Advertisement { WebId = advertisementId, Description = "SqlTestAd", Name = "Campaign Ad" };

            cacheProviderMock.Setup(c => c.GetAdvertisementFromCache(advertisementId)).Returns((Advertisement)null);
            noSqlProviderMock.Setup(n => n.GetAdv(advertisementId, It.IsAny<Queue<DateTime>>())).Returns((Advertisement)null);
            sqlProviderMock.Setup(s => s.GetAdv(advertisementId)).Returns(sqlAdvertisement);

            // Act
            var result = advertisementService.GetAdvertisement(advertisementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sqlAdvertisement, result);

            // Verify that the NoSqlProvider method was called
            noSqlProviderMock.Verify(n => n.GetAdv(advertisementId, It.IsAny<Queue<DateTime>>()), Times.Once);

            // Verify that the SQLProvider method was called
            sqlProviderMock.Verify(s => s.GetAdv(advertisementId), Times.Once);
        }


    }
}