using System;
using System.Collections.Generic;
using System.Linq;
using Meebey.SmartIrc4net;

namespace AgnesBot.Core
{
    public abstract class BaseModule
    {
        private readonly IList<Type> _handlers = new List<Type>();
        
        public void Process(IrcMessageData data)
        {
            var handlers = _handlers.Select(type => (BaseHandler) IoC.Resolve(type))
                .Where(handler => handler.CanHandle(data));

            foreach (var handler in handlers)
                handler.Handle(data);
        }

        protected void AddHandler<THandler>()
        {
            _handlers.Add(typeof(THandler));
        }
    }
}