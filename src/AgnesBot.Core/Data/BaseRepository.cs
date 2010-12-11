using Raven.Client.Document;

namespace AgnesBot.Core.Data
{
    public class BaseRepository
    {
        protected DocumentSession Session
        {
            get { return null; }
        }
    }
}