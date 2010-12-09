using System;
using System.Collections.Generic;
using System.Linq;

namespace AgnesBot.Core
{
    public interface IConfigurationManager
    {
        string Server { get; set; }
        int Port { get; set; }
        string Hostname { get; set; }
        string Email { get; set; }
        string Nickname { get; set; }
        List<string> Channels { get; set; }
        bool AutoJoin { get; set; }
    }

    public class ConfigurationManager : IConfigurationManager
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Hostname { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public List<string> Channels { get; set; }
        public bool AutoJoin { get; set; }

        public ConfigurationManager()
        {
            Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Port"]);
            Hostname = System.Configuration.ConfigurationManager.AppSettings["Hostname"];
            Email = System.Configuration.ConfigurationManager.AppSettings["Email"];
            Nickname = System.Configuration.ConfigurationManager.AppSettings["Nickname"];
            AutoJoin = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["AutoJoin"]);

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Channels"]))
                Channels = System.Configuration.ConfigurationManager.AppSettings["Channels"]
                    .Split(';')
                    .ToList();
        }
    }
}