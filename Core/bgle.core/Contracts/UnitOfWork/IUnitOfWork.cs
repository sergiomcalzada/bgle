using System;
using System.Data;
using System.Data.Common;

namespace bgle.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsInTransaction { get; }

        DbTransaction CurrentTransaction { get; }

        void BeginTransaction();

        void BeginTransaction(IsolationLevel isolationLevel);

        void CommitTransaction();

        void SaveChanges();

        void RollBackTransaction();
    }
}