using bgle.CQRS.Command;
using bgle.CQRS.Event;
using bgle.CQRS.EventPublisher;

namespace bgle.CQRS.CommandHandler
{
    public abstract class BaseEventStorerCommandHandler<TCommand, TEvent> : BaseCommandHandler<TCommand>
        where TCommand : ICommand where TEvent : class, IEvent
    {
        protected readonly IEventStore EventStore;

        private TEvent _event;

        protected BaseEventStorerCommandHandler(IEventStore eventStore)
        {
            this.EventStore = eventStore;
        }

        protected void QueueEvent(TEvent @event)
        {
            this._event = @event;
        }

        protected virtual void Publish()
        {
            this.EventStore.Store(this._event);
        }
    }
}