using System.Collections.Generic;
using Meebey.SmartIrc4net;
using System.Linq;

namespace AgnesBot.Core
{
    public abstract class BaseModule
    {
        private readonly IList<IHandler> _handlers = new List<IHandler>();

        public void Process(IrcMessageData data, IrcClient client)
        {
            var handlers = _handlers.Where(handler => handler.CanHandle(data));

            foreach(var handler in handlers)
                handler.Handle(data, client);
        }

        protected void AddHandler(IHandler handler)
        {
            _handlers.Add(handler);
        }
    }
}