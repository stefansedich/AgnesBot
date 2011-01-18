using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
                .Where(handler => handler.Type == data.Type)
                .ForAll(handler =>
                            {
                                var matches = handler.CommandRegex.Matches(data.Message);

                                foreach (Match match in matches)
                                    ExecuteHandlerMethod(data, handler, match);
                            });
        }

        // TODO: No error checking at the moment, needs work to stop people doing silly things.
        private void ExecuteHandlerMethod(IrcMessageData data, ModuleMessageHandler handler, Match match)
        {
            var method = GetType().GetMethod(handler.Method, BindingFlags.NonPublic | BindingFlags.Instance);

            if(method == null)
                return;

            var arguments = method.GetParameters()
                .Select(parameter =>
                            {
                                if (parameter.Position == 0)
                                    return data;

                                if (match.Groups[parameter.Name] == null)
                                    return Activator.CreateInstance(parameter.ParameterType);

                                return TypeDescriptor.GetConverter(parameter.ParameterType)
                                    .ConvertFromString(match.Groups[parameter.Name].Value);
                            })
                .ToArray();

            method.Invoke(this, arguments);
        }
    }
}