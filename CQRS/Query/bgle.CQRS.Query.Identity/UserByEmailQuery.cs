using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bgle.CQRS.Query.Identity
{
    public class UserByEmailQuery : IQuery
    {
        public string Email { get; private set; }

        public UserByEmailQuery(string email)
        {
            this.Email = email;
        }
    }
}
