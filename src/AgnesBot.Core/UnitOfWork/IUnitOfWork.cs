using System;
using Raven.Client;

namespace AgnesBot.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDocumentSession CurrentSession { get; }

        void SaveChanges();
    }
}