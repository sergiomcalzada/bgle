using System.Linq;

using bgle.Contracts.Specifications;
using bgle.Contracts.UnitOfWork;
using bgle.Entity;

namespace bgle.Contracts.Repository
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }

        TEntity Create<TEntity>() where TEntity : class, IEntity;

        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class, IEntity;

        TEntity Add<TEntity>(TEntity obj) where TEntity : class, IEntity;

        void Remove<TEntity>(TEntity obj) where TEntity : class, IEntity;

        int Count<TEntity>() where TEntity : class, IEntity;

        TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, IEntity;

        bool Any<TEntity>(ISpecification<TEntity> specification) where TEntity : class, IEntity;

        IQueryable<TEntity> Where<TEntity>(ISpecification<TEntity> specification) where TEntity : class, IEntity;
    }
}