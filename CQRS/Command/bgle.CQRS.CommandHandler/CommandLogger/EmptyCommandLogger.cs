using bgle.CQRS.Command;

namespace bgle.CQRS.CommandHandler.CommandLogger
{
    public class EmptyCommandLogger : ICommandLogger
    {
        public void Log(ICommand command)
        {
        }
    }
}