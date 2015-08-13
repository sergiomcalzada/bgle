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
            this.BeforeQuery(query);
            var result = this.Query(query);
            this.AfterQuery(query, result);
            return result;
        }

        protected virtual void BeforeQuery(TQuery query) { }

        protected abstract TQueryResult Query(TQuery query);

        protected virtual void AfterQuery(TQuery query, TQueryResult result) { }
    }
}