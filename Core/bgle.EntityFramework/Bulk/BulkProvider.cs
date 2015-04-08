using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace bgle.EntityFramework.Bulk
{
    public abstract class BulkProvider
    {
        protected readonly DbContext Context;

        private bool ownsConnection;

        private bool ownsTransaction;

        public SqlBulkCopyOptions Options { get; set; }

        protected BulkProvider(DbContext context)
        {
            this.Context = context;
            this.Options = SqlBulkCopyOptions.Default;
        }

        protected void RunInTransaction(Action<IDbTransaction> action)
        {
            this.EnsureConnection();
            var transaction = this.GetTransaction();

            try
            {
                action(transaction);
                if (this.ownsTransaction)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                if (this.ownsTransaction)
                {
                    transaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (this.ownsTransaction && transaction != null)
                {
                    transaction.Dispose();
                }
                this.CloseConnection();
            }
        }

        private void EnsureConnection()
        {
            if (this.Context.Database.Connection.State != ConnectionState.Open)
            {
                this.Context.Database.Connection.Open();
                this.ownsConnection = true;
            }
        }

        private void CloseConnection()
        {
            if (this.ownsConnection)
            {
                this.Context.Database.Connection.Close();
            }
        }

        private DbTransaction GetTransaction()
        {
            var objectContext = ((IObjectContextAdapter)this.Context).ObjectContext;
            var entityTransaction = ((EntityConnection)objectContext.Connection).CurrentTransaction;
            if (entityTransaction != null)
            {
                return entityTransaction.StoreTransaction;
            }
            this.ownsTransaction = true;
            return this.Context.Database.BeginTransaction().UnderlyingTransaction;
        }
    }
}