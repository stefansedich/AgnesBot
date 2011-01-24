using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Modules.GitHubModule.Domain;
using AgnesBot.Modules.GitHubModule.Services;
using Castle.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace AgnesBot.Modules.GitHubModule.Tests
{
    [TestFixture]
    public class GitHubSearchModuleFixture
    {
        private GitHubSearchModule _module;
        private IGitHubService _gitHubService;
        private IIrcClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = MockRepository.GenerateStub<IIrcClient>();
            _gitHubService = MockRepository.GenerateStub<IGitHubService>();
            _module = new GitHubSearchModule(_client, _gitHubService);
        }

        [Test]
        public void Search_Displays_Top_5_Results()
        {
            // Arrange
            const string TERM = "blah blah";

            var data = new IrcMessageData
                           {
                               Type = ReceiveType.ChannelMessage,
                               Message = "!github search " + TERM
                           };

            var projects = new List<Repository>()
                               {
                                   new Repository{Name = "1", Url = "1"},
                                   new Repository{Name = "2", Url = "2"},
                                   new Repository{Name = "3", Url = "3"},
                                   new Repository{Name = "4", Url = "4"},
                                   new Repository{Name = "5", Url = "5"},
                                   new Repository{Name = "6", Url = "6"},
                               };

            _gitHubService.Stub(x => x.Search(TERM))
                .Return(projects);

            // Act
            _module.Process(data);

            // Assert  
            projects.Take(5).ForEach(
                project =>
                _client.AssertWasCalled(
                    x =>
                    x.SendMessage(SendType.Message, data.Channel, string.Format("{0} - {1}", project.Name, project.Url))));

            projects.Skip(5).ForEach(
                project =>
                _client.AssertWasNotCalled(
                    x =>
                    x.SendMessage(SendType.Message, data.Channel, string.Format("{0} - {1}", project.Name, project.Url))));
        }
    }
}
