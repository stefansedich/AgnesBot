using AgnesBot.Core.IrcUtils;
using AgnesBot.Modules.TinyUrlModule.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace AgnesBot.Modules.TinyUrlModule.Tests
{
    [TestFixture]
    public class TinyUrlModuleFixture
    {
        private TinyUrlModule _module;
        private ITinyUrlService _tinyUrlService;
        private IIrcClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = MockRepository.GenerateStub<IIrcClient>();
            _tinyUrlService = MockRepository.GenerateStub<ITinyUrlService>();
            _module = new TinyUrlModule(_client, _tinyUrlService);
        }

        [Test]
        public void Can_Shorten_Url()
        {
            // Arrange
            const string URL = "www.google.com";
            const string SHORTENED_URL = "a";

            var data = new IrcMessageData { Message = "!tinyurl " + URL, Channel = "#test", Type = ReceiveType.ChannelMessage };

            _tinyUrlService.Stub(service => service.ShortenUrl(URL))
                .Return(SHORTENED_URL);

            // Act
            _module.Process(data);

            // Assert
            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Channel, "TinyUrl: " + SHORTENED_URL));
        }
    }
}
