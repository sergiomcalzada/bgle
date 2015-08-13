using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;
using bgle.CQRS.Event;
using bgle.CQRS.EventStore;

namespace bgle.CQRS.CommandHandler.Identity
{
    public class UpdateUserCommandHandler : UpsertCommandHandler<UpdateUserCommand, IdentityUser, string, EmptyEvent>
    {
        public UpdateUserCommandHandler(IEventStore eventStore, IRepository repository, IDateTimeProvider dateTimeProvider)
            : base(eventStore, repository, dateTimeProvider)
        {
        }
        
        protected override void FillEntity(UpdateUserCommand command, IdentityUser entity)
        {
            entity.PasswordHash = command.PasswordHash;
            entity.Email = command.Email;
            entity.SecurityStamp = command.SecurityStamp;
            entity.EmailConfirmed = command.EmailConfirmed;
            entity.LockoutEndDate = command.LockoutEndDate;
            entity.AccessFailedCount = command.AccessFailedCount;
            entity.LockoutEnabled = command.LockoutEnabled;
        }
    }
}