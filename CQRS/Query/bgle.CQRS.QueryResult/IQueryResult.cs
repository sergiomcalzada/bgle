using System.Collections.Generic;

namespace bgle.CQRS.QueryResult
{
    public interface IQueryResult
    {
    }

    public interface IQueryResultList<TQueryResultListItem> : IQueryResult, IList<TQueryResultListItem>
        where TQueryResultListItem : IQueryResultListItem
    {
        void AddRange(IEnumerable<TQueryResultListItem> range);
    }

    public interface IQueryResultListItem : IQueryResult
    {
    }
}