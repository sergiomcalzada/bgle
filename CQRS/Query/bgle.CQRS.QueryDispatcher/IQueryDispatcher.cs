using System.Threading.Tasks;

using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;

namespace bgle.CQRS.QueryDispatcher
{
    public interface IQueryDispatcher
    {
        TQueryResult Dispatch<TQueryParameters, TQueryResult>(TQueryParameters query) where TQueryParameters : IQuery where TQueryResult : IQueryResult;

        Task<TQueryResult> DispatchAsync<TQueryParameters, TQueryResult>(TQueryParameters query) where TQueryParameters : IQuery where TQueryResult : IQueryResult;
    }
}