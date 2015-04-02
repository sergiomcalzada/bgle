using System;

using bgle.CQRS.Query;
using bgle.CQRS.QueryHandler;
using bgle.CQRS.QueryResult;

namespace bgle.CQRS.QueryDispatcher.Factory
{
    public interface IQueryHandlerFactory : IDisposable
    {
        IQueryHandler<TQuery, TQueryResult> ResolveQueryHandler<TQuery, TQueryResult>() where TQuery : IQuery where TQueryResult : IQueryResult;

        void Release(object handler);
    }
}