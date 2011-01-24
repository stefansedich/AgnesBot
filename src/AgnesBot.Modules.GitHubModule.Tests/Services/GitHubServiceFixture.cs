using System.Linq;
using AgnesBot.Modules.GitHubModule.Services;
using NUnit.Framework;

namespace AgnesBot.Modules.GitHubModule.Tests.Services
{
    [Category("Integration")]
    public class GitHubServiceFixture
    {
        private GitHubService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new GitHubService();
        }

        [Test]
        public void Search()
        {
            // Arrange
            const string TERM = "agnes";

            // Act
            var result = _service.Search(TERM);

            // Assert  
            Assert.IsNotEmpty(result.ToList());
        }
    }
}