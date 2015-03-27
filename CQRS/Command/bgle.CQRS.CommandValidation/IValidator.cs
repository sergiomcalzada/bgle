using System.ComponentModel.DataAnnotations;

namespace bgle.CQRS.CommandValidation
{
    public interface IValidator
    {
        ValidationResult Validate();
    }
}