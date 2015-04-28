using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;

namespace bgle.CQRS.QueryHandler
{
    public abstract class BaseQueryHandler<TQuery, TQueryResult> : IQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery where TQueryResult : IQueryResult
    {
        public TQueryResult Handle(TQuery query)
        {
            this.BeforeQuery(query);
            var result = this.Query(query);
            this.AfterQuery(query, result);
            return result;
        }

        protected abstract void BeforeQuery(TQuery query);

        protected abstract TQueryResult Query(TQuery query);

        protected abstract void AfterQuery(TQuery query, TQueryResult result);
    }
}