using bgle.CQRS.QueryDispatcher.Factory;
using bgle.CQRS.QueryHandler;

namespace bgle.CQRS.QueryDispatcher
{
    public class QueryDispatcher : BaseQueryDispatcher
    {
        private readonly IQueryHandlerFactory factory;

        public QueryDispatcher(IQueryHandlerFactory factory)
        {
            this.factory = factory;
        }

        protected override IQueryHandler<TQuery, TQueryResult> GetQueryHandler<TQuery, TQueryResult>()
        {
            return this.factory.ResolveQueryHandler<TQuery, TQueryResult>();
        }
    }
}