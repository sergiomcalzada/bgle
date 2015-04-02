﻿using bgle.CQRS.CommandBus.Factory;
using bgle.CQRS.CommandHandler;
using bgle.CQRS.CommandValidationHandler;

namespace bgle.CQRS.CommandBus
{
    public class CommandBus : BaseCommandBus
    {
        private readonly ICommandHandlerFactory factory;

        public CommandBus(ICommandHandlerFactory factory)
        {
            this.factory = factory;
        }

        protected override ICommandValidationHandler<TCommand> GetValidationHandler<TCommand>()
        {
            return this.factory.ResolveCommandValidationHandler<TCommand>();
        }

        protected override ICommandHandler<TCommand> GetCommandHandler<TCommand>()
        {
            return this.factory.ResolveCommandHandler<TCommand>();
        }
    }
}