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
        private readonly IUrlRepository _urlRepository;

        public UrlAggregatorModule(IIrcClient client, IUrlRepository urlRepository) : base(client)
        {
            _urlRepository = urlRepository;

            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = @"https?://([-\w\.]+)+(:\d+)?(/([\w/_\.]*(\?\S+)?)?)?",
                               Action = AddUrl
                           });
        }

        private void AddUrl(IrcMessageData obj)
        {
            string url = Regex.Match(obj.Message, @"https?://([-\w\.]+)+(:\d+)?(/([\w/_\.]*(\?\S+)?)?)?").Groups[0].Value;

            UnitOfWork.Start(() => _urlRepository.SaveUrl(new Url
                                                              {
                                                                  Link = url,
                                                                  Timestamp = SystemTime.Now()
                                                              }));
        }
    }
}
