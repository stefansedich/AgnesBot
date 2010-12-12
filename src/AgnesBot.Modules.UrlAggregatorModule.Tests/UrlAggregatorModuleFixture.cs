using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using AgnesBot.Modules.UrlAggregatorModule.Domain;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;

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

            // Act
            _module.Process(data);

            // Assert
            _urlRepository.AssertWasCalled(repository => repository.SaveUrl(Arg<Url>.Matches(x =>
                                                                                             x.Link == URL
                                                                                             && x.Timestamp == SystemTime.Now())));
        }
    }
}
