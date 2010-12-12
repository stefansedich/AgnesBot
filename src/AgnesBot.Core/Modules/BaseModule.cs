using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AgnesBot.Core.IrcUtils;

namespace AgnesBot.Core.Modules
{
    public abstract class BaseModule : IModule
    {
        private readonly IList<ModuleMessageHandler> _handlers = new List<ModuleMessageHandler>();

        protected readonly IIrcClient Client;
        
        protected BaseModule(IIrcClient client)
        {
            Client = client;
        }
        
        public void AddHandler(ModuleMessageHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Process(IrcMessageData data)
        {
            foreach (var handler in _handlers)
            {
                if (handler.Type != data.Type)
                    continue;

                var match = handler.CommandRegex.Match(data.Message);
                
                if(match.Success == false)
                    continue;

                var commandData = handler.CommandRegex
                    .GetGroupNames()
                    .ToDictionary(name => name, name => match.Groups[name].Value);

                handler.Action(data, commandData);
            }
                
        }
    }
}