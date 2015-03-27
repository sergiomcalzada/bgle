using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command
{
    public class ValidatableCommand<T> : Command<T>, IValidatableCommand
    {
        public ValidatableCommand()
        {
        }

        public ValidatableCommand(T id)
            : base(id)
        {
        }

        public virtual ValidationResultCollection Validate()
        {
            var vc = new ValidationContext(this);

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, vc, validationResults, true);

            return new ValidationResultCollection(validationResults);
        }
    }
}