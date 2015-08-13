namespace bgle.CQRS.Query.Identity
{
    public class UserByNameQuery : IQuery
    {
        public string UserName { get; private set; }

        public UserByNameQuery(string userName)
        {
            this.UserName = userName;
        }
    }
}