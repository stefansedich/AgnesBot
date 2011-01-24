using System.Linq;
using System.Text.RegularExpressions;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Modules.GitHubModule.Services;

namespace AgnesBot.Modules.GitHubModule
{
    public class GitHubSearchModule : BaseModule
    {
        private readonly IGitHubService _gitHubService;

        public GitHubSearchModule(IIrcClient client, IGitHubService gitHubService) : base(client)
        {
            _gitHubService = gitHubService;
            AddHandler(new ModuleMessageHandler
                           {
                               Type = ReceiveType.ChannelMessage,
                               CommandRegex = new Regex("^!github search (?<term>.+)$"),
                               Method = "SearchGitHub"
                           });
        }

        protected void SearchGitHub(IrcMessageData data, string term)
        {
            var projects = _gitHubService.Search(term)
                .Take(5);

            foreach(var project in projects)
                Client.SendMessage(SendType.Message, data.Channel, string.Format("{0} - {1}", project.Name, project.Url));
        }
    }
}
