using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;

namespace bgle.CQRS.QueryHandler
{
    public abstract class BaseQueryHandler<TQuery, TQueryResult> : IQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
    {
        public TQueryResult Handle(TQuery query)
        {
            this.BeforeHandle(query);
            var result = this.DoHandle(query);
            this.AfterHandle(query, result);
            return result;
        }

        protected abstract void BeforeHandle(TQuery query);

        protected abstract TQueryResult DoHandle(TQuery query);

        protected abstract void AfterHandle(TQuery query, TQueryResult result);
    }
}