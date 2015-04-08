using System;

namespace bgle.CQRS.Query
{
    public interface IDateTimeRangeQuery : IQuery
    {
        DateTime FromDateTime { get; set; }
        DateTime ToDateTime { get; set; }
    }
}