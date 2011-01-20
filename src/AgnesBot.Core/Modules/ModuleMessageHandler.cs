using System.Text.RegularExpressions;
using AgnesBot.Core.IrcUtils;

namespace AgnesBot.Core.Modules
{
    public class ModuleMessageHandler
    {
        public ReceiveType Type { get; set; }
        public Regex CommandRegex { get; set; }
        public string Method { get; set; }
    }
}