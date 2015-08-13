using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult.Identity;
using bgle.CQRS.QueryResultBuilder.Identity;

namespace bgle.CQRS.QueryHandler.Identity
{
    public class UserByIdQueryHandler : EntityByIdQueryHandler<UserQueryResult, IdentityUser, string, UserQueryResultBuilder>
    {
        protected UserByIdQueryHandler(IRepository repository)
            : base(repository)
        {
        }


    }
}
