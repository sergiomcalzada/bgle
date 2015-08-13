using System;

namespace bgle.CQRS.Command.Identity
{
    public class UpdateUserCommand : StringEntityCommand
    {
       
        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }
        
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public DateTimeOffset? LockoutEndDate { get; set; }

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }

        public UpdateUserCommand(string id)
            : base(id)
        {
        }
    }
}