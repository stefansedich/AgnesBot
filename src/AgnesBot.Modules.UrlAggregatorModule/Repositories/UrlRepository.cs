using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.Repositories;
using AgnesBot.Modules.UrlAggregatorModule.Domain;

namespace AgnesBot.Modules.UrlAggregatorModule.Repositories
{
    public class UrlRepository : BaseRepository, IUrlRepository
    {
        public void SaveUrl(Url url)
        {
            Session.Store(url);
        }

        public IList<Url> GetAllUrls()
        {
            return Session.Query<Url>()
                .ToList();
        }

        public bool UrlExists(string link)
        {
            return Session.Query<Url>()
                .Where(x => x.Link == link)
                .FirstOrDefault() != null;
        } 
    }
}