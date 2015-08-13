using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Query.Identity;
using bgle.CQRS.QueryResult.Identity;
using bgle.CQRS.QueryResultBuilder.Identity;
using bgle.CQRS.QuerySpecificationBuilder.Identity;

namespace bgle.CQRS.QueryHandler.Identity
{
    public class UserByEmailQueryHandler : SingleEntitySpecificationQueryHandler<UserByEmailQuery, UserQueryResult, IdentityUser, UserByEmailQuerySpecificationBuilder, UserQueryResultBuilder>
    {
        public UserByEmailQueryHandler(IRepository repository)
            : base(repository)
        {
        }
    }
}