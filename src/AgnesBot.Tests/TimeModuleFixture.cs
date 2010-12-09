using System;
using AgnesBot.Core;
using AgnesBot.Modules;
using Meebey.SmartIrc4net;
using NUnit.Framework;
using Rhino.Mocks;

namespace AgnesBot.Tests
{
    [TestFixture]
    public class TimeModuleFixture
    {
        private TimeModule _module;
        private IIrcClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = MockRepository.GenerateStub<IIrcClient>();
            _module = new TimeModule(_client);
        }

        [Test]
        public void Process_Sends_CurrentTime_To_Channel_If_Message_Is_TimeMessage()
        {
            // Arrange
            SystemTime.Now = () => new DateTime(2000, 1, 1);

            var message = new IrcMessage
                              {
                                  Message = "!time",
                                  Type = ReceiveType.ChannelMessage,
                                  Channel = "#test"
                              };

            // Act
            _module.Process(message);

            // Assert
            _client.AssertWasCalled(client => client.SendMessage(SendType.Message, message.Channel, "The Time Is: " + SystemTime.Now()));
        }
    }
}
