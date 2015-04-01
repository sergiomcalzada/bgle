using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;

using bgle.Contracts.Repository;
using bgle.Contracts.UnitOfWork;
using bgle.CQRS.EntityFramework.Resorces;

namespace bgle.CQRS.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;

        private bool disposed;

        private bool ownedConnection;

        private DbContextTransaction transaction;

        public EntityFrameworkUnitOfWork(IDbContext dbContext)
        {
            this.context = (DbContext)dbContext;
        }

        public DbTransaction CurrentTransaction { get { return this.IsInTransaction ? this.transaction.UnderlyingTransaction : null; } }

        public bool IsInTransaction { get { return this.transaction != null; } }

        public void BeginTransaction()
        {
            this.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.IsInTransaction)
            {
                throw new ApplicationException(ErrorMessages.AlreadyRunningTransaction);
            }
            OpenConnection();
            this.transaction = this.context.Database.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (!this.IsInTransaction)
            {
                throw new ApplicationException(ErrorMessages.NoTransactionForCommit);
            }

            try
            {
                this.transaction.Commit();
                ReleaseCurrentTransaction();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }

        public void RollBackTransaction()
        {
            if (!this.IsInTransaction)
            {
                throw new ApplicationException(ErrorMessages.NoTransactionForRollBack);
            }

            this.transaction.Rollback();
            this.ReleaseCurrentTransaction();
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }


        private void OpenConnection()
        {
            if (this.context.Database.Connection.State != ConnectionState.Open)
            {
                this.context.Database.Connection.Open();
                this.ownedConnection = true;
            }
        }

        private void CloseConnection()
        {
            if (this.ownedConnection)
            {
                this.context.Database.Connection.Close();
                this.ownedConnection = false;
            }
        }

        private void ReleaseCurrentTransaction()
        {
            if (this.IsInTransaction)
            {
                this.transaction.Dispose();
                this.transaction = null;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.CloseConnection();
                }
            }
            disposed = true;
        }

        #endregion
    }
}