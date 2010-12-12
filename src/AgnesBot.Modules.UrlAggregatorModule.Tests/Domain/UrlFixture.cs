using AgnesBot.Modules.UrlAggregatorModule.Domain;
using NUnit.Framework;

namespace AgnesBot.Modules.UrlAggregatorModule.Tests.Domain
{
    [TestFixture]
    public class UrlFixture
    {
        [Test]
        public void SafeUrl_Prepended_With_NSFW_If_Url_Is_NSFW()
        {
            // Arrange
            const string URL = "http://xx.com";

            var url = new Url {Link = URL};
            Assert.AreEqual(url.SafeUrl, URL);

            // Act
            url.Nsfw = true;
            
            // Assert
            Assert.AreEqual("[NSFW] " + URL, url.SafeUrl);
        }
    }
}