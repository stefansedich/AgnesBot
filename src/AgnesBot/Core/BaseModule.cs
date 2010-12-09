using System.Collections.Generic;
using Meebey.SmartIrc4net;
using System.Linq;

namespace AgnesBot.Core
{
    public abstract class BaseModule
    {
        public abstract IList<IHandler> Handlers { get; }

        public void Process(IrcMessageData data, IrcClient client)
        {
            var handlers = Handlers.Where(handler => handler.CanHandle(data));

            foreach(var handler in handlers)
                handler.Handle(data, client);
        }
    }
}