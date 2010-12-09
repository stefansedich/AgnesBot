using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public class AgnesBotRunner
    {
        private readonly List<IModule> _modules = new List<IModule>();
        private readonly IrcClient _irc = new IrcClient();

        public void Start()
        {
            LoadModules();
            ConnectToServer();
        }

        private void LoadModules()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assemblies = Directory.GetFiles(path, "*.dll")
                .Select(assembly => Assembly.LoadFile(assembly))
                .Concat(new List<Assembly> {Assembly.GetExecutingAssembly()});

            var types = assemblies.SelectMany(x => x.GetTypes())
                .Where(type => type.GetInterfaces().Any(intface => typeof(IModule).IsAssignableFrom(type)))
                .Select(type => (IModule) Activator.CreateInstance(type));

            _modules.AddRange(types);
        }

        private void ConnectToServer()
        {
            _irc.OnConnected += OnConnected;
            _irc.OnReadLine += OnReadLine;
            _irc.SendDelay = 500;

            _irc.Connect(ConfigurationManager.AppSettings["Server"],
                         Convert.ToInt32(ConfigurationManager.AppSettings["Port"]));
        }

        void OnConnected(object sender, EventArgs e)
        {
            _irc.Login(ConfigurationManager.AppSettings["Nickname"],
                       ConfigurationManager.AppSettings["Hostname"],
                       0,
                       ConfigurationManager.AppSettings["Email"],
                       String.Empty);

            var channels = ConfigurationManager.AppSettings["Channels"].Split(';');

            foreach(var channel in channels)
                _irc.RfcJoin(channel);
            
            _irc.Listen();
        }

        private void OnReadLine(object sender, ReadLineEventArgs e)
        {
            IrcMessageData data = _irc.MessageParser(e.Line);

            if (_irc.IsMe(data.Nick))
                return;

            var module = _modules.FirstOrDefault(x => x.CanHandle(data));

            if (module != null)
                module.Handle(data, _irc);
        }
    }
}