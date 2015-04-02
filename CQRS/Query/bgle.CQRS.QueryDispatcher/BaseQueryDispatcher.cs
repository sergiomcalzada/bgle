using System;
using System.Threading.Tasks;

using bgle.CQRS.Query;
using bgle.CQRS.QueryHandler;
using bgle.CQRS.QueryResult;

namespace bgle.CQRS.QueryDispatcher
{
    public abstract class BaseQueryDispatcher : IQueryDispatcher
    {
        public TQueryResult Dispatch<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            var queryHandler = this.GetQueryHandler<TQuery, TQueryResult>();

            if (queryHandler == null)
            {
                throw new ArgumentNullException();
            }

            return queryHandler.Handle(query);
        }

        public async Task<TQueryResult> DispatchAsync<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery where TQueryResult : IQueryResult
        {
            return await Task.Factory.StartNew(() => this.Dispatch<TQuery, TQueryResult>(query));
        }

        protected abstract IQueryHandler<TQuery, TQueryResult> GetQueryHandler<TQuery, TQueryResult>() where TQuery : IQuery where TQueryResult : IQueryResult;
    }
}