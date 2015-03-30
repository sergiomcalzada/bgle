using bgle.CQRS.CommandHandler;

namespace bgle.CQRS.CommandBus
{
    public interface IScopedCommandHandlerFactory : ICommandHandlerFactory
    {
        ICommandScope BeginScope();
    }
}