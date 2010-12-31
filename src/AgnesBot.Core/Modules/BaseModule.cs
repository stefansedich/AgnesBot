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
            _handlers.AsParallel()
                .ForAll(handler =>
                             {
                                 if (handler.Type != data.Type)
                                     return;

                                 var matches = handler.CommandRegex.Matches(data.Message);
                                 
                                 foreach (Match match in matches)
                                 {
                                     var commandData = handler.CommandRegex
                                         .GetGroupNames()
                                         .ToDictionary(name => name, name => match.Groups[name].Value);

                                     handler.Action(data, commandData);
                                 }
                             });
        }
    }
}