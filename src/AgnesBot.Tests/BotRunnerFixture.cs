using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using Rhino.Mocks;

namespace AgnesBot.Server.Tests
{
    [TestFixture]
    public class BotRunnerFixture
    {
        private IConfigurationManager _configurationManager;
        private BotRunner _runner;
        private IIrcClient _client;
        private List<IModule> _modules;
        private IModule _module1;
        private IModule _module2;

        const string SERVER = "abc";
        const int PORT = 1234;
        const string NICKNAME = "a";
        const string HOSTNAME = "b";
        const string EMAIL = "c";

        [SetUp]
        public void SetUp()
        {
            _module1 = MockRepository.GenerateStub<IModule>();
            _module2 = MockRepository.GenerateStub<IModule>();
            _modules = new List<IModule> {_module1, _module2};
            
            _configurationManager = MockRepository.GenerateStub<IConfigurationManager>();
            _client = MockRepository.GenerateStub<IIrcClient>();
            _runner = new BotRunner(_configurationManager, _client, _modules);

            _configurationManager.Server = SERVER;
            _configurationManager.Port = PORT;
            _configurationManager.Nickname = NICKNAME;
            _configurationManager.Hostname = HOSTNAME;
            _configurationManager.Email = EMAIL;
            _configurationManager.Channels = new List<string> { "#a", "#b" };
        }

        [Test]
        public void BotRunner_Connects_When_Started()
        {
            // Arrange
            
            // Act
            _runner.Start();

            // Assert
            _client.AssertWasCalled(client => client.Connect(SERVER, PORT));
        }

        [Test]
        public void BotRunner_Logs_In_Once_Connected()
        {
            // Arrange
        
            // Act
            _runner.Start();
            _client.OnConnected();

            // Assert
            _client.AssertWasCalled(client => client.Login(NICKNAME, HOSTNAME, 0, EMAIL, string.Empty));
        }

        [Test]
        public void BotRunner_AutoJoins_Channels_If_AutoJoin_Is_On()
        {
            // Arrange
            _configurationManager.AutoJoin = true;

            // Act
            _runner.Start();
            _client.OnConnected();

            // Assert
            foreach(var channel in _configurationManager.Channels)
                _client.AssertWasCalled(client => client.RfcJoin(channel));
        }

        [Test]
        public void BotRunner_Does_Not_AutoJoin_Channels_If_AutoJoin_Is_Off()
        {
            // Arrange
            _configurationManager.AutoJoin = false;

            // Act
            _runner.Start();
            _client.OnConnected();
            
            // Assert
            foreach (var channel in _configurationManager.Channels)
                _client.AssertWasNotCalled(client => client.RfcJoin(channel));
        }

        [Test]
        public void BotRunner_Calls_Process_On_All_Modules_For_New_Line()
        {
            // Arrange
            var message = new IrcMessageData() {Message = "!test"};
            
            _client.Stub(client => client.MessageParser("message"))
                .Return(message);

            // Act 
            _runner.Start();
            _client.OnReadLine("message");
            
            // Assert
            _module1.AssertWasCalled(module => module.Process(message));
            _module2.AssertWasCalled(module => module.Process(message));
        }

        [Test]
        public void BotRunner_Does_Not_Call_Process_On_Modules_If_Line_Is_From_Myself()
        {
            // Arrange
            var data = new IrcMessageData();
            
            _client.Stub(client => client.MessageParser("message"))
                .Return(data);

            _client.Stub(client => client.IsMe(data.Nickname))
                .Return(true);

            // Act
            _runner.Start();
            _client.OnReadLine("message");

            // Assert
            _module1.AssertWasNotCalled(module => module.Process(data));
            _module2.AssertWasNotCalled(module => module.Process(data));
        }

        [Test]
        public void BotRunner_Does_Not_Process_Empty_Message()
        {
            // Arrange
            var data = new IrcMessageData();
            
            _client.Stub(client => client.MessageParser(""))
                .Return(data);

            _client.Stub(client => client.IsMe(data.Nickname))
                .Return(true);

            // Act
            _runner.Start();
            _client.OnReadLine("");

            // Assert
            _module1.AssertWasNotCalled(module => module.Process(data));
            _module2.AssertWasNotCalled(module => module.Process(data));
        }
    }
}