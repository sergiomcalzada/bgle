using System;

namespace bgle.Entity
{
    public interface IIdentityUser<out TKey>
    {
        TKey Id { get; }

        string UserName { get; set; }

        string PasswordHash { get; set; }

        string SecurityStamp { get; set; }

        string Email { get; set; }

        bool EmailConfirmed { get; set; }

        DateTimeOffset? LockoutEndDate { get; set; }

        int AccessFailedCount { get; set; }

        bool LockoutEnabled { get; set; }
    }
}