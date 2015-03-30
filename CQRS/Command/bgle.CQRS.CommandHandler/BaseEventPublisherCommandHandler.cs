using bgle.CQRS.Command;
using bgle.CQRS.Event;

namespace bgle.CQRS.CommandHandler
{
    public abstract class BaseEventPublisherCommandHandler<TCommand, TEvent> : BaseCommandHandler<TCommand>
        where TCommand : ICommand where TEvent : class, IEvent
    {
        protected readonly IEventPublisher EventPublisher;

        private TEvent _event;

        protected BaseEventPublisherCommandHandler(IEventPublisher eventPublisher)
        {
            this.EventPublisher = eventPublisher;
        }

        protected void QueueEvent(TEvent @event)
        {
            this._event = @event;
        }

        protected virtual void Publish()
        {
            this.EventPublisher.Publish(this._event);
        }
    }
}