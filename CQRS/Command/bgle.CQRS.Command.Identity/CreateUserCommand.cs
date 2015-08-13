using System.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command.Identity
{
    public class CreateUserCommand : StringEntityCommand
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        [Required]
        public string Email { get; set; }

        public string EmailConfirmed { get; set; }

        public string LockoutEndDate { get; set; }

        public string AccessFailedCount { get; set; }

        public string LockoutEnabled { get; set; }

        public CreateUserCommand(string userName, string passwordHash, string email)
        {
            this.UserName = userName;
            this.PasswordHash = passwordHash;
            this.Email = email;
        }
    }
}