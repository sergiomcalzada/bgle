using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;

namespace bgle.CQRS.CommandValidationHandler.Identity
{
    public class UpdateUserCommandValidationHandler : UpdateEntityCommandValidationHandler<CreateUserCommand, IdentityUser, string>
    {
        public UpdateUserCommandValidationHandler(IRepository repository)
            : base(repository)
        {
        }
    }
}