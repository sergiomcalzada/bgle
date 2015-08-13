using System;

namespace bgle.CQRS.QueryResult.Identity
{
    public class UserQueryResult : IQueryResult
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTimeOffset? LockoutEndDate { get; set; }

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }
    }
}
