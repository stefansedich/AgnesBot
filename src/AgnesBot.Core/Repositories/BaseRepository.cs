using AgnesBot.Core.UnitOfWork;
using Raven.Client;

namespace AgnesBot.Core.Repositories
{
    public class BaseRepository
    {
        protected IDocumentSession Session
        {
            get { return UnitOfWorkFactory.CurrentUnitOfWork.CurrentSession; }
        }
    }
}