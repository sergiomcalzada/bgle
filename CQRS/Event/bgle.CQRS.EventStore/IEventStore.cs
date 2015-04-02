using bgle.CQRS.Event;

namespace bgle.CQRS.EventPublisher
{
    public interface IEventStore
    {
        void Store<TEvent>(TEvent entityEvent) where TEvent : class, IEvent;
    }
}