using System;

using bgle.Contracts.DateTimeHandling;
using bgle.CQRS.Command;
using bgle.CQRS.CommandHandler.CommandLogger;
using bgle.CQRS.Event;
using bgle.CQRS.EventPublisher;

namespace bgle.CQRS.CommandHandler
{
    public abstract class GivenDoPublishCommandHandler<TCommand, TEvent> : BaseEventStorerCommandHandler<TCommand, TEvent>
        where TCommand : ICommand where TEvent : class, IEvent
    {
        protected readonly IDateTimeProvider DateTimeProvider;

        private readonly ICommandLogger commandLogger;

        protected GivenDoPublishCommandHandler(IEventStore eventStore, IDateTimeProvider dateTimeProvider, ICommandLogger commandLogger)
            : base(eventStore)
        {
            this.DateTimeProvider = dateTimeProvider;
            this.commandLogger = commandLogger;
        }

        protected override void BeforeHandle(TCommand command)
        {
            this.Given(command);
        }

        protected override void InHandle(TCommand command)
        {
            this.Do(command);
            this.QueueEvent(this.CreateEvent(command));
        }

        protected override void AfterHandle(TCommand command)
        {
            this.Publish();
            this.LogCommand(command);
        }

        protected TEvent CreateEvent(TCommand command)
        {
            var @event = this.InstanceEvent(command);
            this.FillEvent(@event, command);
            return @event;
        }

        protected virtual TEvent InstanceEvent(TCommand command)
        {
            var @event = Activator.CreateInstance<TEvent>();
            return @event;
        }

        protected virtual void LogCommand(TCommand command)
        {
            this.commandLogger.Log(command);
        }

        protected abstract void Given(TCommand command);

        protected abstract void Do(TCommand command);

        protected abstract void FillEvent(TEvent @event, TCommand command);
    }
}