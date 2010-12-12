using System;
using System.Collections.Generic;
using System.Linq;
using AgnesBot.Core.Data;

namespace AgnesBot.Modules.UrlAggregatorModule.Domain
{
    public interface IUrlRepository
    {
        void SaveUrl(Url url);
        IList<Url> GetAllUrls();
    }

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
    }
}