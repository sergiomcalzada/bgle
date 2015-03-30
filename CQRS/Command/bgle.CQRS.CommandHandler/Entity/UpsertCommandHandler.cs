using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.CQRS.Command;
using bgle.CQRS.CommandHandler.CommandLogger;
using bgle.CQRS.Event;
using bgle.Entity;

namespace bgle.CQRS.CommandHandler
{
    public abstract class UpsertCommandHandler<TCommand, TEntity, TEntityType, TEvent> : EntityCommandHandler<TCommand, TEntity, TEntityType, TEvent>
        where TCommand : IEntityCommand<TEntityType> where TEntity : class, IEntity<TEntityType> where TEvent : class, IEntityEvent<TEntityType>
    {
        protected UpsertCommandHandler(IEventPublisher eventPublisher, IRepository repository, IDateTimeProvider dateTimeProvider, ICommandLogger commandLogger)
            : base(eventPublisher, repository, dateTimeProvider, commandLogger)
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