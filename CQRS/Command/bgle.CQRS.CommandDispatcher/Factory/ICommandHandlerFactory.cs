using System;

using bgle.CQRS.Command;
using bgle.CQRS.CommandHandler;
using bgle.CQRS.CommandValidationHandler;

namespace bgle.CQRS.CommandDispatcher.Factory
{
    public interface ICommandHandlerFactory : IDisposable
    {
        ICommandHandler<TCommand> ResolveCommandHandler<TCommand>() where TCommand : ICommand;

        ICommandValidationHandler<TCommand> ResolveCommandValidationHandler<TCommand>() where TCommand : IValidatableCommand;

        void Release(object handler);
    }
}