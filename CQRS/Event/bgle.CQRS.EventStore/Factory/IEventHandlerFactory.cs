using System;

using bgle.CQRS.Event;
using bgle.CQRS.EventHandler;

namespace bgle.CQRS.EventStore.Factory
{
    public interface IEventHandlerFactory : IDisposable
    {
        IEventHandler<TEvent>[] ResolveAllEventHandlers<TEvent>() where TEvent : IEvent;

        void Release(object handler);
    }
}