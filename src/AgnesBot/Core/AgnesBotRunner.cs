using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public class AgnesBotRunner
    {
        private Configuration _configuration;
        private readonly List<BaseModule> _modules = new List<BaseModule>();
        private readonly IrcClient _irc = new IrcClient();

        public void Start()
        {
            LoadConfiguration();
            LoadModules();
            ConnectToServer();
        }

        private void LoadConfiguration()
        {
            _configuration = new Configuration();
        }

        private void LoadModules()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assemblies = Directory.GetFiles(path, "*.dll")
                .Select(assembly => Assembly.LoadFile(assembly))
                .Concat(new List<Assembly> {Assembly.GetExecutingAssembly()});

            var types = assemblies.SelectMany(x => x.GetTypes())
                .Where(type => typeof(BaseModule).IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => (BaseModule) Activator.CreateInstance(type));

            _modules.AddRange(types);
        }

        private void ConnectToServer()
        {
            _irc.OnConnected += OnConnected;
            _irc.OnReadLine += OnReadLine;
            _irc.SendDelay = 500;

            _irc.Connect(_configuration.Server, _configuration.Port);
        }

        void OnConnected(object sender, EventArgs e)
        {
            _irc.Login(_configuration.Nickname, _configuration.Hostname, 0, _configuration.Email, String.Empty);
            
            if(_configuration.AutoJoin)
            {
                foreach (var channel in _configuration.Channels)
                    _irc.RfcJoin(channel);
            }
            
            _irc.Listen();
        }

        private void OnReadLine(object sender, ReadLineEventArgs e)
        {
            IrcMessageData data = _irc.MessageParser(e.Line);

            if (_irc.IsMe(data.Nick))
                return;

            _modules.ForEach(module => module.Process(data, _irc));
        }
    }
}