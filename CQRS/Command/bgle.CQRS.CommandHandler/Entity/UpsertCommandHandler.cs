using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.CQRS.Command;
using bgle.CQRS.Event;
using bgle.CQRS.EventStore;
using bgle.Entity;

namespace bgle.CQRS.CommandHandler
{
    public abstract class UpsertCommandHandler<TCommand, TEntity, TEntityType, TEvent> : EntityCommandHandler<TCommand, TEntity, TEntityType, TEvent>
        where TCommand : IEntityCommand<TEntityType> where TEntity : class, IEntity<TEntityType> where TEvent : class, IEntityEvent<TEntityType>
    {
        protected UpsertCommandHandler(IEventStore eventStore, IRepository repository, IDateTimeProvider dateTimeProvider)
            : base(eventStore, repository, dateTimeProvider)
        {
        }

        protected override void Do(TCommand command, TEntity entity)
        {
            this.FillEntity(command, entity);

            if (command.IsTransient)
            {
                this.Repository.Add(entity);
            }
        }

        protected abstract void FillEntity(TCommand command, TEntity entity);
    }
}