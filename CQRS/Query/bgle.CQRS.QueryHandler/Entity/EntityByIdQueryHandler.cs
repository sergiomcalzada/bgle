using bgle.Contracts.Repository;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;
using bgle.CQRS.QueryResultBuilder;
using bgle.CQRS.QuerySpecificationBuilder;
using bgle.Entity;

namespace bgle.CQRS.QueryHandler
{
    public abstract class EntityByIdQueryHandler<TQueryResult, TEntity, TKey, TQueryResultBuilder> : SingleEntitySpecificationQueryHandler<
                                                                                                         EntityByIdQuery<TKey>,
                                                                                                         TQueryResult,
                                                                                                         TEntity,
                                                                                                         QueryByIdSpecificationBuilder<EntityByIdQuery<TKey>, TEntity, TKey>, TQueryResultBuilder>
        where TQueryResult : IQueryResult
        where TEntity : class, IEntity<TKey>
        where TQueryResultBuilder : IEntityQueryResultBuilder<TQueryResult, TEntity>, new()
    {
        protected EntityByIdQueryHandler(IRepository repository)
            : base(repository)
        {
        }


    }
}