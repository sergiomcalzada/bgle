using System;
using System.Threading.Tasks;

using bgle.ComponentModel.DataAnnotations;
using bgle.CQRS.Command;
using bgle.CQRS.CommandHandler;
using bgle.CQRS.CommandValidationHandler;

namespace bgle.CQRS.CommandBus
{
    public abstract class BaseCommandBus : ICommandBus
    {
        public virtual ValidationResultCollection Submit<TCommand>(TCommand command) where TCommand : class, IValidatableCommand
        {
            if (command == null)
            {
                throw new ArgumentNullException();
            }

            var commandValidationResult = this.Validate(command);

            if (commandValidationResult.IsValid)
            {
                this.Send(command);
            }

            return commandValidationResult;
        }

        public virtual async Task<ValidationResultCollection> SubmitAsync<TCommand>(TCommand command) where TCommand : class, IValidatableCommand
        {
            return await Task.Factory.StartNew(() => this.Submit(command));
        }

        protected ValidationResultCollection Validate<TCommand>(TCommand command) where TCommand : IValidatableCommand
        {
            var commandValidationHandler = this.GetValidationHandler<TCommand>();

            if (commandValidationHandler == null)
            {
                throw new ArgumentNullException();
            }

            return commandValidationHandler.Validate(command);
        }

        protected void Send<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            var commandHandler = this.GetCommandHandler<TCommand>();

            if (commandHandler == null)
            {
                throw new ArgumentNullException();
            }

            this.Send(command, commandHandler);
        }

        protected virtual void Send<TCommand>(TCommand command, ICommandHandler<TCommand> commandHandler) where TCommand : class, ICommand
        {
            commandHandler.Handle(command);
        }

        protected abstract ICommandValidationHandler<TCommand> GetValidationHandler<TCommand>() where TCommand : IValidatableCommand;

        protected abstract ICommandHandler<TCommand> GetCommandHandler<TCommand>() where TCommand : ICommand;
    }
}