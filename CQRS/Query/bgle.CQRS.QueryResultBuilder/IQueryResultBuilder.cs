using bgle.CQRS.QueryResult;
using bgle.Entity;

namespace bgle.CQRS.QueryResultBuilder
{
    public interface IQueryResultBuilder<out TQueryResult, in TEntity>
        where TQueryResult : IQueryResult
        where TEntity : IEntity
    {
        TQueryResult Build(TEntity entity);
    }
}