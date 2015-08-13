using bgle.Contracts.Specifications;
using bgle.Contracts.Specifications.Entity;
using bgle.CQRS.Query;
using bgle.Entity;

namespace bgle.CQRS.QuerySpecificationBuilder
{
    public class QueryByIdSpecificationBuilder<TQuery, TEntity, TKey> : IQuerySpecificationBuilder<TQuery, TEntity>
        where TQuery : IQuery, IEntityByIdQuery<TKey>
        where TEntity : class, IEntity<TKey>
    {
        public virtual ISpecification<TEntity> Build(TQuery query)
        {
            return new EntityByIdSpecification<TEntity, TKey>(query.Id);
        }
    }

    public class QueryByIdSpecificationBuilder<TQuery, TEntity> : QueryByIdSpecificationBuilder<TQuery, TEntity, string>
        where TQuery : IEntityByIdQuery<string>
        where TEntity : class, IEntity<string>
    {
    }
}