using bgle.CQRS.EventHandler;
using bgle.CQRS.EventPublisher.Factory;

namespace bgle.CQRS.EventPublisher
{
    public class EventStore : BaseEventStore
    {
        private readonly IEventHandlerFactory factory;

        public EventStore(IEventHandlerFactory factory)
        {
            this.factory = factory;
        }

        protected override IEventHandler<TEvent>[] GetEventHandlers<TEvent>()
        {
            return this.factory.ResolveAllEventHandlers<TEvent>();
        }
    }
}