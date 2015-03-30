using bgle.CQRS.Command;

namespace bgle.CQRS.CommandHandler
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        void Handle(TCommand command);
    }
}