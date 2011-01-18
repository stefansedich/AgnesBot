using System;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace AgnesBot.Modules.DateModule.Tests
{
    [TestFixture]
    public class DateModuleFixture
    {
        private DateModule _module;
        private IIrcClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = MockRepository.GenerateStub<IIrcClient>();
            _module = new DateModule(_client);
        }

        [Test]
        public void Can_Display_Date()
        {
            // Arrange
            var data = new IrcMessageData {Channel = "#test", Type = ReceiveType.ChannelMessage, Message = "!date"};

            SystemTime.Now = () => new DateTime(2009, 1, 1, 10, 22, 34);

            // Act
            _module.Process(data);

            // Assert
            string expected = SystemTime.Now().ToString("yyyy-MM-dd hh:mm:sstt");

            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, data.Channel, expected));
        }
    }
}
