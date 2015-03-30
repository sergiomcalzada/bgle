using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using bgle.ComponentModel.DataAnnotations;

namespace bgle.CQRS.CommandValidation
{
    public class ValidatorFactory
    {
        private IList<ValidatorWrapper> ValidatorWrappers { get; set; }

        public ValidatorFactory()
        {
            this.ValidatorWrappers = new List<ValidatorWrapper>();
        }

        public ValidatorFactory Add(IValidator validator, bool continueOnNotSuccess = false)
        {
            this.ValidatorWrappers.Add(new ValidatorWrapper(validator, continueOnNotSuccess));

            return this;
        }

        public ValidationResultCollection Validate()
        {
            var commandValidationResult = new ValidationResultCollection();
            foreach (var validatorWrapper in this.ValidatorWrappers)
            {
                var validationResult = validatorWrapper.Validator.Validate();

                if (validationResult == ValidationResult.Success)
                {
                    continue;
                }

                commandValidationResult.ValidationResults.Add(validationResult);

                if (!validatorWrapper.ContinueOnNotSuccess)
                {
                    break;
                }
            }

            return commandValidationResult;
        }

        private class ValidatorWrapper
        {
            public IValidator Validator { get; private set; }

            public bool ContinueOnNotSuccess { get; private set; }

            public ValidatorWrapper(IValidator validator, bool continueOnNotSuccess)
            {
                this.Validator = validator;
                this.ContinueOnNotSuccess = continueOnNotSuccess;
            }
        }
    }
}