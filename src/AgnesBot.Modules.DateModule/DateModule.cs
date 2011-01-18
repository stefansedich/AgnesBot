using System.Text.RegularExpressions;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.Utils;

namespace AgnesBot.Modules.DateModule
{
    public class DateModule : BaseModule
    {
        public DateModule(IIrcClient client) : base(client)
        {
            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = new Regex("^!date$"),
                               Method = "DisplayDate"
                           });
        }

        protected void DisplayDate(IrcMessageData data)
        {
            string date = SystemTime.Now().ToString("yyyy-MM-dd hh:mm:sstt");

            Client.SendMessage(SendType.Message, data.Channel, date);
        }
    }
}
