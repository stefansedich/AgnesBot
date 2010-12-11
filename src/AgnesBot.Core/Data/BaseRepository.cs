using AgnesBot.Core.UnitOfWork;
using Raven.Client;

namespace AgnesBot.Core.Data
{
    public class BaseRepository
    {
        protected IDocumentSession Session
        {
            get { return UnitOfWorkFactory.CurrentUnitOfWork.CurrentSession; }
        }
    }
}