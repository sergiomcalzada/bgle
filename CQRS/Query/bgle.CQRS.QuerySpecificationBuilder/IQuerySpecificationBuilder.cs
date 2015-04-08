using bgle.Contracts.Specifications;
using bgle.CQRS.Query;
using bgle.Entity;

namespace bgle.CQRS.QuerySpecificationBuilder
{
    public interface IQuerySpecificationBuilder<in TQuery, TEntity>
        where TQuery : IQuery where TEntity : IEntity
    {
        ISpecification<TEntity> Build(TQuery query);
    }
}