using bgle.ComponentModel.DataAnnotations;
using bgle.Contracts.Repository;
using bgle.CQRS.Command;
using bgle.CQRS.CommandValidation;

namespace bgle.CQRS.CommandValidationHandler
{
    public abstract class EntityCommandValidationHandler<TCommand, TEntity, TKey> : BaseCommandValidationHandler<TCommand>
        where TCommand : IEntityCommand<TKey>, IValidatableCommand
    {
        protected readonly IRepository Repository;

        protected EntityCommandValidationHandler(IRepository repository)
        {
            this.Repository = repository;
        }

        public override ValidationResultCollection Validate(TCommand command)
        {
            var commandValidationResult = base.Validate(command);

            if (commandValidationResult.IsValid)
            {
                var factory = new ValidatorFactory();
                this.BeforeBuildValidator(factory, command);
                this.BuildValidator(factory, command);
                commandValidationResult = factory.Validate();
            }

            return commandValidationResult;
        }

        protected virtual void BeforeBuildValidator(ValidatorFactory factory, TCommand command)
        {
        }

        protected abstract void BuildValidator(ValidatorFactory factory, TCommand command);
    }
}