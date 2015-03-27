namespace bgle.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        void SaveChanges();

        void BeginTransaction();

        void CommitTransaction();

        void RollBackTransaction();
    }
}