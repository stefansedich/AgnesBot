using System.Collections.Generic;
using AgnesBot.Modules.UrlAggregatorModule.Domain;

namespace AgnesBot.Modules.UrlAggregatorModule.Repositories
{
    public interface IUrlRepository
    {
        void SaveUrl(Url url);
        IList<Url> GetAllUrls();
        bool UrlExists(string link);
    }
}