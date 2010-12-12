using System.Collections.Generic;
using System.Text.RegularExpressions;
using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Modules.UrlAggregatorModule.Domain;

namespace AgnesBot.Modules.UrlAggregatorModule
{
    public class UrlAggregatorModule : BaseModule
    {
        private readonly Regex _urlMatchRegex = new Regex(@"(?<url>https?://([-\w\.]+)+(:\d+)?(/([\w/_\.]*(\?\S+)?)?)?)");
        private readonly IUrlRepository _urlRepository;

        public UrlAggregatorModule(IIrcClient client, IUrlRepository urlRepository) : base(client)
        {
            _urlRepository = urlRepository;

            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = _urlMatchRegex,
                               Action = AddUrl
                           });

            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = new Regex("^!links$"),
                               Action = ListLinks
                           });
        }

        private void ListLinks(IrcMessageData data, IDictionary<string, string> commandData)
        {
            UnitOfWork.Start(() =>
                                 {
                                     var urls = _urlRepository.GetAllUrls();

                                     foreach (var url in urls)
                                         Client.SendMessage(SendType.Message, data.Nickname, url.SafeUrl);
                                 });
        }

        private void AddUrl(IrcMessageData data, IDictionary<string, string> commandData)
        {
            var matches = _urlMatchRegex.Matches(data.Message);

            UnitOfWork.Start(() =>
                                 {
                                     foreach (Match match in matches)
                                     {
                                         string url = match.Groups["url"].Value;

                                         _urlRepository.SaveUrl(new Url
                                                                    {
                                                                        Link = url,
                                                                        Timestamp = SystemTime.Now(),
                                                                        NSFW =
                                                                            Regex.IsMatch(data.Message, "nsfw",
                                                                                          RegexOptions.IgnoreCase)
                                                                    });
                                     }
                                 });

        }
    }
}
