using bgle.Contracts.Repository;
using bgle.Contracts.Specifications;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;
using bgle.CQRS.QuerySpecificationBuilder;
using bgle.Entity;

namespace bgle.CQRS.QueryHandler
{
    public abstract class EntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder> : EntityQueryHandler<TQuery, TQueryResult, TEntity>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
        where TEntity : class, IEntity
        where TQuerySpecificationBuilder : IQuerySpecificationBuilder<TQuery, TEntity>, new()
    {
        protected ISpecification<TEntity> Specification;

        protected EntitySpecificationQueryHandler(IRepository repository)
            : base(repository)
        {
        }

        protected override void BeforeHandle(TQuery query)
        {
            this.Specification = new TQuerySpecificationBuilder().Build(query);
        }

    }
}