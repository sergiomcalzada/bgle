using bgle.Contracts.Specifications;
using bgle.Core.Identity;
using bgle.CQRS.Query.Identity;

namespace bgle.CQRS.QuerySpecificationBuilder.Identity
{
    public class UserByEmailQuerySpecificationBuilder : IQuerySpecificationBuilder<UserByEmailQuery, IdentityUser>
    {
        public ISpecification<IdentityUser> Build(UserByEmailQuery query)
        {
            return new Specification<IdentityUser>(x => x.Email == query.Email);
        }
    }
}