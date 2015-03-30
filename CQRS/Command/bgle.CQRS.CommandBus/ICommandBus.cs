using bgle.ComponentModel.DataAnnotations;
using bgle.CQRS.Command;

namespace bgle.CQRS.CommandBus
{
    public interface ICommandBus
    {
        ValidationResultCollection Submit<TCommand>(TCommand command) where TCommand : class, IValidatableCommand;
    }
}