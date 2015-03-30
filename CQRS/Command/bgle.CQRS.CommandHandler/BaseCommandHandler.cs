using bgle.CQRS.Command;

namespace bgle.CQRS.CommandHandler
{
    public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public void Handle(TCommand command)
        {
            this.BeforeHandle(command);
            this.InHandle(command);
            this.AfterHandle(command);
        }

        protected abstract void BeforeHandle(TCommand command);

        protected abstract void InHandle(TCommand command);

        protected abstract void AfterHandle(TCommand command);
    }
}