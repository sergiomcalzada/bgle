using bgle.CQRS.QueryResult;
using bgle.Entity;

namespace bgle.CQRS.QueryResultBuilder
{
    public interface IEntityQueryResultBuilder<out TQueryResult, in TEntity> : IQueryResultBuilder<TQueryResult, TEntity>
        where TQueryResult : IQueryResult
        where TEntity : IEntity
    { 
        TQueryResult Build(TEntity entity);
    }
}