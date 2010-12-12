using System;
using System.Linq;
using System.Text.RegularExpressions;
using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using AgnesBot.Modules.UrlAggregatorModule.Domain;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;
using System.Collections.Generic;

namespace AgnesBot.Modules.UrlAggregatorModule.Tests
{
    [TestFixture]
    public class UrlAggregatorModuleFixture
    {
        private IIrcClient _client;
        private UrlAggregatorModule _module;
        private IUrlRepository _urlRepository;

        [SetUp]
        public void SetUp()
        {
            _urlRepository = MockRepository.GenerateStub<IUrlRepository>();
            _client = MockRepository.GenerateStub<IIrcClient>();
            _module = new UrlAggregatorModule(_client, _urlRepository);

            var unitOfWork = MockRepository.GenerateStub<IUnitOfWork>();
            var unitOfWorkFactory = MockRepository.GenerateStub<IUnitOfWorkFactory>();
            unitOfWorkFactory.Stub(x => x.Create()).Return(unitOfWork);

            var container = MockRepository.GenerateStub<IWindsorContainer>();
            container.Stub(x => x.Resolve<IUnitOfWorkFactory>())
                .Return(unitOfWorkFactory);

            IoC.Initialize(container);
        }
        
        [Test]
        public void Can_Add_Links_From_Message()
        {
            // Arrange
            const string URL = "http://bob.com";

            var data = new IrcMessageData { Type = ReceiveType.ChannelMessage, Message = "cool site " + URL, Channel = "#test" };

            SystemTime.Now = () => new DateTime(2009, 1, 1);

            // Act
            _module.Process(data);

            // Assert
            _urlRepository.AssertWasCalled(repository => repository.SaveUrl(Arg<Url>.Matches(x =>
                                                                                             x.Link == URL
                                                                                             && x.Timestamp == SystemTime.Now())));
        }

        [Test]
        public void Links_Flagged_NSFW_If_Message_Contains_NSFW()
        {
            // Arrange
            const string URL = "http://bob.com";

            var data = new IrcMessageData { Type = ReceiveType.ChannelMessage, Message = "nsfw " + URL, Channel = "#test" };

            SystemTime.Now = () => new DateTime(2009, 1, 1);

            // Act
            _module.Process(data);

            // Assert
            _urlRepository.AssertWasCalled(repository => repository.SaveUrl(Arg<Url>.Matches(x =>
                                                                                             x.NSFW
                                                                                             && x.Link == URL
                                                                                             && x.Timestamp == SystemTime.Now())));
        }

        [Test]
        public void Can_Handle_Multiple_Links()
        {
            // Arrange
            const string URL1 = "http://bob.com";
            const string URL2 = "http://foo.com";

            var data = new IrcMessageData { Type = ReceiveType.ChannelMessage, Message = "bleh " + URL1 + " awesome pic " + URL2, Channel = "#test" };

            SystemTime.Now = () => new DateTime(2009, 1, 1);

            // Act
            _module.Process(data);

            // Assert
            _urlRepository.AssertWasCalled(repository => repository.SaveUrl(null), opt =>
                                                                                       {
                                                                                           opt.IgnoreArguments();
                                                                                           opt.Repeat.Times(2);
                                                                                       });

        }

        [Test]
        public void User_Can_Get_List_Of_Links()
        {
            // Arrange
            var data = new IrcMessageData { Type = ReceiveType.ChannelMessage, Message = "!links", Nickname = "bob"};

            var links = new List<Url>
                            {
                                new Url {Link = "a"},
                                new Url {Link = "b", NSFW = true}
                            };

            _urlRepository.Stub(repository => repository.GetAllUrls())
                .Return(links);

            // Act
            _module.Process(data);

            // Assert
            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Nickname, links[0].SafeUrl));
            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Nickname, links[1].SafeUrl));
        }
    }
}
