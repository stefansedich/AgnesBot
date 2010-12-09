using System;
using AgnesBot.Core;
using Meebey.SmartIrc4net;

namespace AgnesBot.Modules
{
    public class TimeAndDateModule : BaseModule
    {
        public TimeAndDateModule()
        {
            AddHandler(new TimeMessageHandler());
        }
    }

    public class TimeMessageHandler : HandlerBase
    {
        public TimeMessageHandler()
        {
            Handles(ReceiveType.ChannelMessage)
                .WithText("!time");
        }

        public override void Handle(IrcMessageData data, IrcClient client)
        {
            client.SendMessage(SendType.Message, data.Channel, "Time: " + DateTime.Now);
        }
    }
}