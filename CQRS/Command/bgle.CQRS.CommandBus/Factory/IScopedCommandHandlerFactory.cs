using bgle.CQRS.CommandHandler;

namespace bgle.CQRS.CommandBus.Factory
{
    public interface IScopedCommandHandlerFactory : ICommandHandlerFactory
    {
        ICommandScope BeginScope();
    }
}