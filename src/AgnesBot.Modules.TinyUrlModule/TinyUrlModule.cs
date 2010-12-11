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
                               CommandRegex = "^!tinyurl",
                               Action = ShortenUrl
                           });
        }

        private void ShortenUrl(IrcMessageData data)
        {
            string url = data.MessageWithoutCommand;

            if (string.IsNullOrEmpty(url))
                return;

            string shortenedUrl = _tinyUrlService.ShortenUrl(url);

            Client.SendMessage(SendType.Message, data.Channel, "TinyUrl: " + shortenedUrl);
        }
    }
}
