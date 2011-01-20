using System;
using AgnesBot.Core.Utils;
using Raven.Client;

namespace AgnesBot.Core.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDocumentSession CurrentSession { get; protected set; }

        public UnitOfWork(IDocumentSession currentSession)
        {
            CurrentSession = currentSession;
        }

        public static IUnitOfWork Start()
        {
            return IoC.Resolve<IUnitOfWorkFactory>()
                .Create();
        }

        public static void Start(Action action)
        {
            using(var uow = Start())
            {
                action();

                uow.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            CurrentSession.SaveChanges();
        }

        public void Dispose()
        {
            if (CurrentSession != null)
                CurrentSession.Dispose();
            
            UnitOfWorkFactory.CloseCurrentUnitOfWork();
        }
    }
}