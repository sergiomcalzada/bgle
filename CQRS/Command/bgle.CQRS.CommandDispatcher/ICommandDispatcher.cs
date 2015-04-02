using bgle.ComponentModel.DataAnnotations;
using bgle.CQRS.Command;

namespace bgle.CQRS.CommandBus
{
    public interface ICommandDispatcher
    {
        ValidationResultCollection Dispatch<TCommand>(TCommand command) where TCommand : class, IValidatableCommand;
    }
}