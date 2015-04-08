using System.Collections.Generic;

namespace bgle.CQRS.QueryResult
{
    public interface IQueryResultCollection<TQueryResultListItem> : IQueryResult, ICollection<TQueryResultListItem>
        where TQueryResultListItem : IQueryResultCollectionItem
    {
        void AddRange(IEnumerable<TQueryResultListItem> range);
    }
}