using bgle.CQRS.EventHandler;
using bgle.CQRS.EventStore.Factory;

namespace bgle.CQRS.EventStore
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