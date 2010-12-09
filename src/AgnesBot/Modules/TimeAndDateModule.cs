using System;
using System.Collections.Generic;
using AgnesBot.Core;
using Meebey.SmartIrc4net;

namespace AgnesBot.Modules
{
    public class TimeAndDateModule : BaseModule
    {
        public override IList<IHandler> Handlers
        {
            get
            {
                return new List<IHandler>
                           {
                               new TimeHandler()
                           };
            }
        }
    }

    public class TimeHandler : IHandler
    {
        public bool CanHandle(IrcMessageData data)
        {
            return data.Type == ReceiveType.ChannelMessage 
                   && data.MessageArray[0] == "!time";
        }

        public void Handle(IrcMessageData data, IrcClient client)
        {
            client.SendMessage(SendType.Message, data.Channel, "Time: " + DateTime.Now);
        }
    }
}