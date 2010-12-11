using System;
using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.Irc;

namespace AgnesBot.Core.Modules
{
    public abstract class BaseModule : IModule
    {
        private readonly IList<KeyValuePair<Func<IrcMessageData, bool>, Action<IrcMessageData>>> _handlers;

        protected readonly IIrcClient Client;
        
        protected BaseModule(IIrcClient client)
        {
            Client = client;

            _handlers = new List<KeyValuePair<Func<IrcMessageData, bool>, Action<IrcMessageData>>>();
        }
        
        public void AddHandler(Func<IrcMessageData, bool> predicate, Action<IrcMessageData> handler)
        {
            _handlers.Add(new KeyValuePair<Func<IrcMessageData, bool>, Action<IrcMessageData>>(predicate, handler));
        }

        public void Process(IrcMessageData data)
        {
            var handlers = _handlers.Where(handler => handler.Key(data));

            foreach (var handler in handlers)
                handler.Value(data);
        }
    }
}