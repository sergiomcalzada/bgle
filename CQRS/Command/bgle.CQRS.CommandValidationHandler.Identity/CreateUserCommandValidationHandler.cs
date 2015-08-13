using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;

namespace bgle.CQRS.CommandValidationHandler.Identity
{
    public class CreateUserCommandValidationHandler : CreateEntityCommandValidationHandler<CreateUserCommand, IdentityUser, string>
    {
        public CreateUserCommandValidationHandler(IRepository repository)
            : base(repository)
        {
        }
    }
}