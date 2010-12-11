using System;
using AgnesBot.Core.IrcUtils;

namespace AgnesBot.Core.Modules
{
    public class ModuleMessageHandler
    {
        public ReceiveType Type { get; set; }
        public string CommandRegex { get; set; }
        public Action<IrcMessageData> Action { get; set; }
    }
}