using System.Linq;

using bgle.Entity;

namespace bgle.Contracts.Specifications
{
    public interface ISpecification<TEntity>
        where TEntity : IEntity
    {
        IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
    }
}