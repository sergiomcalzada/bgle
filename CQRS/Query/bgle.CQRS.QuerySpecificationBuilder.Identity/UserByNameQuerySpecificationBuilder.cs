using bgle.Contracts.Specifications;
using bgle.Core.Identity;
using bgle.CQRS.Query.Identity;

namespace bgle.CQRS.QuerySpecificationBuilder.Identity
{
    public class UserByNameQuerySpecificationBuilder : IQuerySpecificationBuilder<UserByNameQuery,IdentityUser>
    {
        public ISpecification<IdentityUser> Build(UserByNameQuery query)
        {
            return new Specification<IdentityUser>(x => x.UserName == query.UserName);
        }
    }
}
