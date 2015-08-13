using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.Core.Identity;
using bgle.CQRS.Command.Identity;
using bgle.CQRS.Event;
using bgle.CQRS.EventStore;

namespace bgle.CQRS.CommandHandler.Identity
{
    public class DeleteUserCommandHandler : DeleteCommandHandler<DeleteUserCommand, IdentityUser, string, EmptyEvent>
    {
        public DeleteUserCommandHandler(IEventStore eventStore, IRepository repository, IDateTimeProvider dateTimeProvider)
            : base(eventStore, repository, dateTimeProvider)
        {
        }
    }
}