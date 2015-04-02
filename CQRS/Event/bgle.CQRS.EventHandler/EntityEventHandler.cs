using bgle.Contracts.Repository;
using bgle.CQRS.Event;

namespace bgle.CQRS.EventHandler
{
    public abstract class EntityEventHandler<TEvent, TKey> : BaseEventHandler<TEvent>
        where TEvent : IEntityEvent<TKey>
    {
        protected readonly IRepository Repository;

        protected EntityEventHandler(IRepository repository)
        {
            this.Repository = repository;
        }
    }
}