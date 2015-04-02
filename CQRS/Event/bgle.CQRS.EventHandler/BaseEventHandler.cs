using bgle.CQRS.Event;

namespace bgle.CQRS.EventHandler
{
    public abstract class BaseEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract void Handle(TEvent @event);
    }
}