using System.ComponentModel.DataAnnotations;

namespace bgle.CQRS.CommandValidation
{
    public abstract class BaseValidator : IValidator
    {
        private readonly string errorMessage;
        private readonly string[] memberNames;

        protected BaseValidator(string errorMessage, params string[] memberNames)
        {
            this.errorMessage = errorMessage;
            this.memberNames = memberNames;
        }

        public ValidationResult Validate()
        {
            if (!this.IsValid())
            {
                return new ValidationResult(this.errorMessage, this.memberNames);
            }

            return null;
        }

        protected abstract bool IsValid();
    }
}