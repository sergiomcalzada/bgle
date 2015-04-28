using System;

using bgle.CQRS.QueryResult;
using bgle.Entity;

namespace bgle.CQRS.QueryResultBuilder
{
    public interface IExpressionQueryResultBuilder<out TQueryResult, in TEntity> : IQueryResultBuilder<TQueryResult, TEntity>
        where TQueryResult : IQueryResult
        where TEntity : IEntity
    {
        Func<TEntity, TQueryResult> Selector();
    }
}