using System;

namespace bgle.Contracts.DateTimeHandling
{
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get { return DateTime.UtcNow; } }

        public DateTime Epoch { get { return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); } }
    }
}