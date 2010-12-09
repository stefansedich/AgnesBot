using System;
using AgnesBot.Core;
using AgnesBot.Modules;
using Meebey.SmartIrc4net;
using System.Collections.Generic;

namespace AgnesBot
{
    public class BotRunner
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IIrcClient _client;
        
        public BotRunner(IConfigurationManager configurationManager, IIrcClient client)
        {
            _configurationManager = configurationManager;
            _client = client;
        }

        public void Start()
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            _client.OnConnected = () => OnConnected();
            _client.OnReadLine = line => OnReadLine(line);
            
            _client.Connect(_configurationManager.Server, _configurationManager.Port);
        }

        void OnConnected()
        {
            _client.Login(_configurationManager.Nickname, _configurationManager.Hostname, 0, _configurationManager.Email, String.Empty);
            
            if(_configurationManager.AutoJoin)
            {
                foreach (var channel in _configurationManager.Channels)
                    _client.RfcJoin(channel);
            }

            _client.Listen();
        }

        private void OnReadLine(string line)
        {
            var message = _client.MessageParser(line);

            if (_client.IsMe(message.Nick))
                return;

            foreach (var handler in IoC.Resolve<IEnumerable<BaseModule>>())
                handler.Process(message);
        }
    }
}