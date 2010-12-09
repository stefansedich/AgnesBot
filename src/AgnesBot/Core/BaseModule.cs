using System.Collections.Generic;
using Meebey.SmartIrc4net;
using System.Linq;

namespace AgnesBot.Core
{
    public abstract class BaseModule
    {
        private readonly IList<HandlerBase> _handlers = new List<HandlerBase>();

        public void Process(IrcMessageData data, IrcClient client)
        {
            var handlers = _handlers.Where(handler => handler.CanHandle(data));

            foreach(var handler in handlers)
                handler.Handle(data, client);
        }

        protected void AddHandler(HandlerBase handler)
        {
            _handlers.Add(handler);
        }
    }
}