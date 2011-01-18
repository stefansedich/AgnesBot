using System.Text.RegularExpressions;
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
                               CommandRegex = new Regex(@"(?<link>https?://([-\w\.]+)+(:\d+)?(/([\w/_\.]*(\?\S+)?)?)?)"),
                               Method = "AddLink"
                           });

            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = new Regex("^!links$"),
                               Method = "ListLinks"
                           });
        }

        protected void ListLinks(IrcMessageData data)
        {
            UnitOfWork.Start(() =>
                                 {
                                     var urls = _urlRepository.GetAllUrls();

                                     foreach (var url in urls)
                                         Client.SendMessage(SendType.Message, data.Nickname, url.SafeLink);
                                 });
        }

        protected void AddLink(IrcMessageData data, string link)
        {
            bool nsfw = Regex.IsMatch(data.Message, "nsfw", RegexOptions.IgnoreCase);

            UnitOfWork.Start(() =>
                                 {
                                     if (_urlRepository.UrlExists(link))
                                         return;

                                     _urlRepository.SaveUrl(new Url
                                                                {
                                                                    Link = link,
                                                                    Timestamp = SystemTime.Now(),
                                                                    Nsfw = nsfw
                                                                });
                                 });
        }
    }
}
