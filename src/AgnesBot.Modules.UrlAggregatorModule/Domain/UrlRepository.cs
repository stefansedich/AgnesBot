using AgnesBot.Core.Data;

namespace AgnesBot.Modules.UrlAggregatorModule.Domain
{
    public interface IUrlRepository
    {
        void SaveUrl(Url url);
    }

    public class UrlRepository : BaseRepository, IUrlRepository
    {
        public void SaveUrl(Url url)
        {
            Session.Store(url);
        }
    }
}