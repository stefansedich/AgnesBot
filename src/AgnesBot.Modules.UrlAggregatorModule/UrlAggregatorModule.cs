using System.Collections.Generic;
using System.Text.RegularExpressions;
using AgnesBot.Core;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using AgnesBot.Modules.UrlAggregatorModule.Domain;
using AgnesBot.Modules.UrlAggregatorModule.Repositories;

namespace AgnesBot.Modules.UrlAggregatorModule
{
    public class UrlAggregatorModule : BaseModule
    {
        private readonly IUrlRepository _urlRepository;

        public UrlAggregatorModule(IIrcClient client, IUrlRepository urlRepository) : base(client)
        {
            _urlRepository = urlRepository;

            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = new Regex(@"https?://([-\w\.]+)+(:\d+)?(/([\w/_\.]*(\?\S+)?)?)?"),
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
                                         Client.SendMessage(SendType.Message, data.Nickname, url.SafeLink);
                                 });
        }

        private void AddUrl(IrcMessageData data, IDictionary<string, string> commandData)
        {
            var matches = Regex.Matches(data.Message, @"(?<url>https?://([-\w\.]+)+(:\d+)?(/([\w/_\.]*(\?\S+)?)?)?)");

            UnitOfWork.Start(() =>
                                 {
                                     foreach (Match match in matches)
                                     {
                                         string url = match.Groups["url"].Value;
                                         bool nsfw = Regex.IsMatch(data.Message, "nsfw", RegexOptions.IgnoreCase);
                                         
                                         if(_urlRepository.GetUrlByLink(url) != null)
                                             continue;

                                         _urlRepository.SaveUrl(new Url
                                                                    {
                                                                        Link = url,
                                                                        Timestamp = SystemTime.Now(),
                                                                        Nsfw = nsfw
                                                                    });
                                     }
                                 });
        }
    }
}
