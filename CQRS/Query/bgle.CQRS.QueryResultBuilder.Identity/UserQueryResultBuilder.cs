using bgle.Core.Identity;
using bgle.CQRS.QueryResult.Identity;

namespace bgle.CQRS.QueryResultBuilder.Identity
{
    public class UserQueryResultBuilder : IEntityQueryResultBuilder<UserQueryResult, IdentityUser>
    {
        public UserQueryResult Build(IdentityUser entity)
        {
            return new UserQueryResult
                       {
                           Id = entity.Id,
                           UserName = entity.UserName,
                           PasswordHash = entity.PasswordHash,
                           SecurityStamp = entity.SecurityStamp,
                           Email = entity.Email,
                           EmailConfirmed = entity.EmailConfirmed,
                           LockoutEndDate = entity.LockoutEndDate,
                           AccessFailedCount = entity.AccessFailedCount,
                           LockoutEnabled = entity.LockoutEnabled,
                       };
        }
    }
}
