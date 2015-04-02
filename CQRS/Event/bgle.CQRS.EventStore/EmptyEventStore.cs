using bgle.CQRS.Event;

namespace bgle.CQRS.EventPublisher
{
    public class EmptyEventStore : IEventStore
    {
        public void Store<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
        }
    }
}