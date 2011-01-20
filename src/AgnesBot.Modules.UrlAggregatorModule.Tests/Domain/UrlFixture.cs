using AgnesBot.Modules.UrlAggregatorModule.Domain;
using NUnit.Framework;

namespace AgnesBot.Modules.UrlAggregatorModule.Tests.Domain
{
    [TestFixture]
    public class UrlFixture
    {
        [Test]
        public void SafeLink_Prepens_Link_With_NSFW_If_Url_Is_Nsfw()
        {
            // Arrange
            const string URL = "http://xx.com";

            var url = new Url {Link = URL};
            Assert.AreEqual(url.SafeLink, URL);

            url.Nsfw = true;

            // Act
            string safeLink = url.SafeLink;
            
            // Assert
            Assert.AreEqual("[NSFW] " + URL, safeLink);
        }

        [Test]
        public void SafeLink_Returns_Link_If_Url_Is_Sfw()
        {
            // Arrange
            var url = new Url {Link = "http://google.com"};

            // Act
            string safeLink = url.SafeLink;

            // Assert
            Assert.AreEqual(url.Link, safeLink);
        }
    }
}