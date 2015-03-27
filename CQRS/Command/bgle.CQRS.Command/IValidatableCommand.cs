using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command
{
    public interface IValidatableCommand : ICommand
    {
        ValidationResultCollection Validate();
    }
}