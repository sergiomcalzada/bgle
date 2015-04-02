using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;

namespace bgle.CQRS.QueryHandler
{
    public interface IQueryHandler<in TQuery, out TQueryResult>
        where TQuery : IQuery where TQueryResult : IQueryResult
    {
        TQueryResult Handle(TQuery query);
    }
}