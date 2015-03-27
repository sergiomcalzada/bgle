using System;

namespace bgle.Contracts.DateTimeHandling
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime Epoch { get; }
    }
}