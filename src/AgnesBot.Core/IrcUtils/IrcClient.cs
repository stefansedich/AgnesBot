using System;

namespace AgnesBot.Core.IrcUtils
{
    public interface IIrcClient
    {
        void Connect(string server, int port);
        void Login(string nickname, string hostname, int i, string email, string empty);
        void RfcJoin(string channel);
        void Listen();
        void SendMessage(SendType type, string destination, string message);
        bool IsMe(string nick);
        IrcMessageData MessageParser(string line);

        Action OnConnected { get; set; }
        Action<string> OnReadLine { get; set; }
    }

    public class IrcClient : IIrcClient
    {
        private readonly Meebey.SmartIrc4net.IrcClient _client;
        
        public IrcClient()
        {
            _client = new Meebey.SmartIrc4net.IrcClient {SendDelay = 500};

            _client.OnConnected += (sender, e) => OnConnected();
            _client.OnReadLine += (sender, e) => OnReadLine(e.Line);
        }

        public void Connect(string server, int port)
        {
            _client.Connect(server, port);
        }

        public void Login(string nickname, string hostname, int i, string email, string empty)
        {
            _client.Login(nickname, hostname, i, email, empty);
        }

        public void RfcJoin(string channel)
        {
            _client.RfcJoin(channel);
        }

        public void Listen()
        {
            _client.Listen();
        }

        public IrcMessageData MessageParser(string line)
        {
            var data = _client.MessageParser(line);

            return new IrcMessageData
                       {
                           Message = data.Message,
                           Channel = data.Channel,
                           From = data.From,
                           Type = (ReceiveType) data.Type,
                           Nickname = data.Nick
                       };
        }
        
        public bool IsMe(string nick)
        {
            return _client.IsMe(nick);
        }

        public void SendMessage(SendType type, string destination, string message)
        {
            _client.SendMessage((Meebey.SmartIrc4net.SendType) type, destination, message);
        }

        public Action OnConnected { get; set; }
        public Action<string> OnReadLine { get; set; }
    }
}