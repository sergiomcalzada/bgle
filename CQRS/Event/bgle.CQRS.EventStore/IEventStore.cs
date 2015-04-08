using bgle.CQRS.Event;

namespace bgle.CQRS.EventStore
{
    public interface IEventStore
    {
        void Store<TEvent>(TEvent entityEvent) where TEvent : class, IEvent;
    }
}