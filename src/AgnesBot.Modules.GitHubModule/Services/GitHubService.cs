using System.Collections.Generic;
using System.Net;
using AgnesBot.Modules.GitHubModule.Domain;
using Newtonsoft.Json;

namespace AgnesBot.Modules.GitHubModule.Services
{
    public class GitHubService : IGitHubService
    {
        public IList<Repository> Search(string term)
        {
            var client = new WebClient();

            string result = client.DownloadString("http://github.com/api/v2/json/repos/search/" + term);
            
            return JsonConvert.DeserializeAnonymousType(result, new {Repositories = new List<Repository>()})
                .Repositories;
        }
    }
}