using bgle.ComponentModel.DataAnnotations;
using bgle.CQRS.Command;

namespace bgle.CQRS.CommandValidationHandler
{
    public interface ICommandValidationHandler<in TCommand>
        where TCommand : IValidatableCommand
    {
        ValidationResultCollection Validate(TCommand command);
    }
}