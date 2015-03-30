using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command
{
    public abstract class ValidatableCommand<T> : Command<T>, IValidatableCommand
    {
        protected ValidatableCommand()
        {
        }

        protected ValidatableCommand(T id)
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