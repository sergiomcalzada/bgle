using System;

using bgle.Entity;

namespace bgle.Core.Identity
{
    public class IdentityUser : DomainEntity, IIdentityUser<string>
    {
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