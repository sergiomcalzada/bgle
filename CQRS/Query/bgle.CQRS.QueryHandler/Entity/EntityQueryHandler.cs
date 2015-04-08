using bgle.Contracts.Repository;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;
using bgle.Entity;

namespace bgle.CQRS.QueryHandler
{
    public abstract class EntityQueryHandler<TQuery, TQueryResult, TEntity> : BaseQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery where TQueryResult : IQueryResult where TEntity : IEntity
    {
        protected readonly IRepository Repository;

        protected EntityQueryHandler(IRepository repository)
        {
            this.Repository = repository;
        }
    }
}