using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace AgnesBot.Core
{
    public class Configuration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Hostname { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public List<string> Channels { get; set; }
        public bool AutoJoin { get; set; }

        public Configuration()
        {
            Server = ConfigurationManager.AppSettings["Server"];
            Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            Hostname = ConfigurationManager.AppSettings["Hostname"];
            Email = ConfigurationManager.AppSettings["Email"];
            Nickname = ConfigurationManager.AppSettings["Nickname"];
            AutoJoin = Convert.ToBoolean(ConfigurationManager.AppSettings["AutoJoin"]);

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["Channels"]))
                Channels = ConfigurationManager.AppSettings["Channels"]
                    .Split(';')
                    .ToList();
        }
    }
}