using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public class IrcMessage
    {
        public string Message { get; set; }
        
        public string[] MessageParts { 
            get
            {
                return string.IsNullOrEmpty(Message) 
                    ? new string[] { }
                    : Message.Split(' ');
            }
        }

        public string From { get; set; }
        public string Channel { get; set; }
        public string Nick { get; set; }
        public ReceiveType Type { get; set; }
    }
}