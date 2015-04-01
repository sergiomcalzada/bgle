using System.Data;
using System.Data.Common;
using System.Data.Entity;

using bgle.Contracts.Repository;
using bgle.Contracts.UnitOfWork;

namespace bgle.CQRS.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;

        public EntityFrameworkUnitOfWork(IDbContext dbContext)
        {
            this.dbContext = (DbContext)dbContext;
        }

        public DbTransaction CurrentTransaction
        {
            get { return this.dbContext.Database.CurrentTransaction == null ? null : this.dbContext.Database.CurrentTransaction.UnderlyingTransaction; }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public void BeginTransaction()
        {
            this.dbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CommitTransaction()
        {
            this.dbContext.Database.CurrentTransaction.Commit();
        }

        public void RollBackTransaction()
        {
            this.dbContext.Database.CurrentTransaction.Rollback();
        }
    }
}