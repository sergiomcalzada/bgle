using System.Data.Common;

namespace bgle.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        DbTransaction CurrentTransaction { get; }

        void SaveChanges();

        void BeginTransaction();

        void CommitTransaction();

        void RollBackTransaction();
    }
}