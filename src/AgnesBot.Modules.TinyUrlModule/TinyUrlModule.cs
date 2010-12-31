using System.Collections.Generic;
using System.Text.RegularExpressions;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Modules.TinyUrlModule.Services;

namespace AgnesBot.Modules.TinyUrlModule
{
    public class TinyUrlModule : BaseModule
    {
        private readonly ITinyUrlService _tinyUrlService;

        public TinyUrlModule(IIrcClient client, ITinyUrlService tinyUrlService) : base(client)
        {
            _tinyUrlService = tinyUrlService;

            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = new Regex("^!tinyurl (?<url>.+)"),
                               Action = ShortenUrl
                           });
        }

        private void ShortenUrl(IrcMessageData data, IDictionary<string, string> commandData)
        {
            string url = commandData["url"];
            string shortenedUrl = _tinyUrlService.ShortenUrl(url);

            Client.SendMessage(SendType.Message, data.Channel, "TinyUrl: " + shortenedUrl);
        }
    }
}
