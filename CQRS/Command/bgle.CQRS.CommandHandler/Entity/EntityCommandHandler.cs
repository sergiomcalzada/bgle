using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.Contracts.Specifications;
using bgle.Contracts.Specifications.Entity;
using bgle.CQRS.Command;
using bgle.CQRS.CommandHandler.CommandLogger;
using bgle.CQRS.Event;
using bgle.CQRS.EventPublisher;
using bgle.Entity;

namespace bgle.CQRS.CommandHandler
{
    public abstract class EntityCommandHandler<TCommand, TEntity, TKey, TEvent> : GivenDoPublishCommandHandler<TCommand, TEvent>
        where TCommand : IEntityCommand<TKey> where TEntity : class, IEntity<TKey> where TEvent : class, IEntityEvent<TKey>
    {
        protected readonly IRepository Repository;

        private TEntity entity;

        protected EntityCommandHandler(IEventStore eventStore, IRepository repository, IDateTimeProvider dateTimeProvider, ICommandLogger commandLogger)
            : base(eventStore, dateTimeProvider, commandLogger)
        {
            this.Repository = repository;
        }

        protected override void Given(TCommand command)
        {
            this.entity = this.GivenEntity(command);
        }

        protected override void Do(TCommand command)
        {
            this.Do(command, this.entity);
        }

        protected override void AfterHandle(TCommand command)
        {
            this.Repository.UnitOfWork.SaveChanges();
            base.AfterHandle(command);
        }

        protected override TEvent InstanceEvent(TCommand command)
        {
            var @event = base.InstanceEvent(command);
            @event.Id = command.Id;
            return @event;
        }

        protected abstract void Do(TCommand command, TEntity entity);

        protected virtual TEntity GivenEntity(TCommand command)
        {
            var e = this.GetEntity(command);

            var now = DateTimeProvider.Now;

            if (command.IsTransient)
            {
                e.Id = command.Id;
                e.CreatedDate = now;
            }

            e.UpdatedDate = now;

            return e;
        }

        protected virtual TEntity GetEntity(TCommand command)
        {
            return command.IsTransient ? this.Repository.Create<TEntity>() : this.Repository.Single(this.GivenEntitySpecification(command));
        }

        protected virtual Specification<TEntity> GivenEntitySpecification(TCommand command)
        {
            return new EntityByIdSpecification<TEntity, TKey>(command.Id);
        }

        protected override void FillEvent(TEvent @event, TCommand command)
        {
            @event.Id = command.Id;
        }
    }
}