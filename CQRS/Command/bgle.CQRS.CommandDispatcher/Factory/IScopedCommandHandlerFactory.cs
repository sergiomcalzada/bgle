using bgle.CQRS.CommandHandler;

namespace bgle.CQRS.CommandDispatcher.Factory
{
    public interface IScopedCommandHandlerFactory : ICommandHandlerFactory
    {
        ICommandScope BeginScope();
    }
}