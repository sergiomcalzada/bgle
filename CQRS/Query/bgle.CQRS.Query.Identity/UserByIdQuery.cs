namespace bgle.CQRS.Query.Identity
{
    public class UserByIdQuery : EntityByIdQuery
    {
        public UserByIdQuery(string id)
            : base(id)
        {
        }
    }
}