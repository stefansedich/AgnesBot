using System;
using AgnesBot.Core;
using Meebey.SmartIrc4net;

namespace AgnesBot.MessageHandlers
{
    public class TimeModule : BaseModule
    {
        public TimeModule(IrcClient client) : base(client) { }

        public override bool CanProcess(IrcMessageData data)
        {
            return data.Type == ReceiveType.ChannelMessage
                   && data.Message.StartsWith("!time");
        }

        public override void Process(IrcMessageData data)
        {
            Client.SendMessage(SendType.Message, data.Channel, "Time: " + DateTime.Now);
        }
    }
}