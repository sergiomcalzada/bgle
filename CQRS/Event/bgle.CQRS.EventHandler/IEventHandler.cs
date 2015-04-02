using bgle.CQRS.Event;

namespace bgle.CQRS.EventHandler
{
    public interface IEventHandler<in TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}