using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;

namespace bgle.CQRS.CommandValidationHandler.Identity
{
    public class DeleteUserCommandValidationHandler : DeleteEntityCommandValidationHandler<DeleteUserCommand, IdentityUser, string>
    {
        public DeleteUserCommandValidationHandler(IRepository repository)
            : base(repository)
        {
        }
    }
}