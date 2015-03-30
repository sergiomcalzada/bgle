using bgle.CQRS.Command;

namespace bgle.CQRS.CommandHandler.CommandLogger
{
    public interface ICommandLogger
    {
        void Log(ICommand command);
    }
}