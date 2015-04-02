using System;
using System.Threading.Tasks;

using bgle.CQRS.Event;
using bgle.CQRS.EventHandler;

namespace bgle.CQRS.EventPublisher
{
    public abstract class BaseEventStore : IEventStore
    {
        public virtual void Store<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            var eventHandlers = this.GetEventHandlers<TEvent>();

            foreach (var eventHandler in eventHandlers)
            {
                if (eventHandler == null)
                {
                    throw new ArgumentNullException();
                }

                this.Store(@event, eventHandler);
            }
        }

        protected virtual void Store<TEvent>(TEvent @event, IEventHandler<TEvent> eventHandler) where TEvent : class, IEvent
        {
            eventHandler.Handle(@event);
        }

        public virtual async Task StoreAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            await Task.Factory.StartNew(() => this.Store(@event));
        }

        protected abstract IEventHandler<TEvent>[] GetEventHandlers<TEvent>() where TEvent : class, IEvent;

        public void Store(IEvent entityEvent)
        {
            throw new NotImplementedException();
        }
    }
}