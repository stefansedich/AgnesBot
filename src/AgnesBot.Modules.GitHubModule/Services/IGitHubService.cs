using System.Collections.Generic;
using AgnesBot.Modules.GitHubModule.Domain;

namespace AgnesBot.Modules.GitHubModule.Services
{
    public interface IGitHubService
    {
        IList<Repository> Search(string term);
    }
}