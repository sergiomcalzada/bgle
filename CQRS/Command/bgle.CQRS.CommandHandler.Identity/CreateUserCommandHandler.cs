using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;
using bgle.CQRS.Event;
using bgle.CQRS.EventStore;

namespace bgle.CQRS.CommandHandler.Identity
{
    public class CreateUserCommandHandler : EntityCommandHandler<CreateUserCommand, IdentityUser, string, EmptyEvent>
    {
        public CreateUserCommandHandler(IEventStore eventStore, IRepository repository, IDateTimeProvider dateTimeProvider)
            : base(eventStore, repository, dateTimeProvider)
        {
        }

        protected override void Do(CreateUserCommand command, IdentityUser entity)
        {
            entity.UserName = command.UserName;
            entity.UserName = command.PasswordHash;
            entity.UserName = command.SecurityStamp;
            entity.UserName = command.Email;
            entity.UserName = command.EmailConfirmed;
            entity.UserName = command.LockoutEndDate;
            entity.UserName = command.AccessFailedCount;
            entity.UserName = command.LockoutEnabled;
        }
    }
}