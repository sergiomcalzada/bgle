using bgle.ComponentModel.DataAnnotations;
using bgle.CQRS.Command;

namespace bgle.CQRS.CommandValidationHandler
{
    public abstract class BaseCommandValidationHandler<TCommand> : ICommandValidationHandler<TCommand>
        where TCommand : IValidatableCommand
    {
        public virtual ValidationResultCollection Validate(TCommand command)
        {
            return command.Validate();
        }
    }
}