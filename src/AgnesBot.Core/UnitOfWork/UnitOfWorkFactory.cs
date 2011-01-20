using System;
using System.Runtime.Remoting.Messaging;
using Raven.Client;

namespace AgnesBot.Core.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IDocumentStore _documentStore;

        private const string UNIT_OF_WORK_KEY = "UnitOfWorkFactory.CurrentUnitOfWork";
        
        internal static IUnitOfWork InternalCurrentUnitOfWork
        {
            get { return CallContext.GetData(UNIT_OF_WORK_KEY) as IUnitOfWork; }
            set { CallContext.SetData(UNIT_OF_WORK_KEY, value); }
        }

        public static IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                if(InternalCurrentUnitOfWork == null)
                    throw new InvalidOperationException("You are not currently in a UnitOfWork");

                return InternalCurrentUnitOfWork;
            }
        }

        public UnitOfWorkFactory(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public IUnitOfWork Create()
        {
            if (InternalCurrentUnitOfWork != null)
                throw new InvalidOperationException("Cannot create a UnitOfWork while another is running");

            var session = _documentStore.OpenSession();
            
            return (InternalCurrentUnitOfWork = new UnitOfWork(session));
        }

        public static void SetGlobalUnitOfWork(IUnitOfWork current)
        {
            InternalCurrentUnitOfWork = current;
        }

        public static void CloseCurrentUnitOfWork()
        {
            InternalCurrentUnitOfWork = null;
        }
    }
}