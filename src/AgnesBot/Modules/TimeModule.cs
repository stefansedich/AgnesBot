using AgnesBot.Core;
using Meebey.SmartIrc4net;

namespace AgnesBot.Modules
{
    public class TimeModule : BaseModule
    {
        public TimeModule(IIrcClient client) : base(client) { }
        
        public override void Process(IrcMessage message)
        {
            if (message.Type != ReceiveType.ChannelMessage)
                return;

            if (message.MessageParts[0] != "!time")
                return;

            Client.SendMessage(SendType.Message, message.Channel, "The Time Is: " + SystemTime.Now());
        }
    }
}