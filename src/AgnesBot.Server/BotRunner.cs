using System;
using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.Utils;

namespace AgnesBot.Server
{
    public class BotRunner
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IIrcClient _client;
        private readonly IList<IModule> _modules;

        public BotRunner(IConfigurationManager configurationManager, IIrcClient client, IList<IModule> modules)
        {
            _configurationManager = configurationManager;
            _client = client;
            _modules = modules;
        }

        public void Start()
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
            var data = _client.MessageParser(line);

            if (string.IsNullOrEmpty(data.Message) || _client.IsMe(data.Nickname))
                return;

            _modules.AsParallel()
                .ForAll(module => module.Process(data));
        }
    }
}