using System;
using AgnesBot.Core;
using Meebey.SmartIrc4net;

namespace AgnesBot.MessageHandlers
{
    public class TimeMessageHandler : BaseHandler
    {
        public TimeMessageHandler(IrcClient client) : base(client) { }

        public override bool CanHandle(IrcMessageData data)
        {
            return data.Type == ReceiveType.ChannelMessage
                   && data.Message.StartsWith("!time");
        }

        public override void Handle(IrcMessageData data)
        {
            Client.SendMessage(SendType.Message, data.Channel, "Time: " + DateTime.Now);
        }
    }
}