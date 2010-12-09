using System;
using Meebey.SmartIrc4net;
using System.Collections.Generic;

namespace AgnesBot.Core
{
    public class AgnesBotRunner
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IrcClient _client;
        
        public AgnesBotRunner(IConfigurationManager configurationManager, IrcClient client)
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
            _client.OnConnected += OnConnected;
            _client.OnReadLine += OnReadLine;
            _client.SendDelay = 500;

            _client.Connect(_configurationManager.Server, _configurationManager.Port);
        }

        void OnConnected(object sender, EventArgs e)
        {
            _client.Login(_configurationManager.Nickname, _configurationManager.Hostname, 0, _configurationManager.Email, String.Empty);
            
            if(_configurationManager.AutoJoin)
            {
                foreach (var channel in _configurationManager.Channels)
                    _client.RfcJoin(channel);
            }

            _client.Listen();
        }

        private void OnReadLine(object sender, ReadLineEventArgs e)
        {
            IrcMessageData data = _client.MessageParser(e.Line);

            if (_client.IsMe(data.Nick))
                return;

            foreach (var module in IoC.Resolve<IEnumerable<BaseModule>>())
                module.Process(data);
        }
    }
}