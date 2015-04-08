using bgle.CQRS.CommandDispatcher.Factory;
using bgle.CQRS.CommandHandler;
using bgle.CQRS.CommandValidationHandler;

namespace bgle.CQRS.CommandDispatcher
{
    public class CommandDispatcher : BaseCommandDispatcher
    {
        private readonly ICommandHandlerFactory factory;

        public CommandDispatcher(ICommandHandlerFactory factory)
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