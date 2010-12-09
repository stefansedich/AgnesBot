using Raven.Client.Document;

namespace AgnesBot.Repositories
{
    public class BaseRepository
    {
        protected DocumentStore Store { get; set; }

        public BaseRepository(DocumentStore store)
        {
            Store = store;
        }
    }
}