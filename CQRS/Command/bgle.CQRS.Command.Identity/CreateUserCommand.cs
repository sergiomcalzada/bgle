using System;
using System.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command.Identity
{
    public class CreateUserCommand : StringEntityCommand
    {
        [Required]
        public string UserName { get; private set; }

        [Required]
        public string PasswordHash { get; private set; }

        public string SecurityStamp { get; set; }

        [Required]
        public string Email { get; private set; }

        public bool EmailConfirmed { get; set; }

        public DateTimeOffset? LockoutEndDate { get; set; }

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }

        public CreateUserCommand(string userName, string passwordHash, string email)
        {
            this.UserName = userName;
            this.PasswordHash = passwordHash;
            this.Email = email;
        }
    }
}