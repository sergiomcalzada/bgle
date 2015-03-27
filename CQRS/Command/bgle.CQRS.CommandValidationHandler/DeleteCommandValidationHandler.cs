namespace bgle.CQRS.CommandValidationHandler
{
    public abstract class DeleteEntityCommandValidationHandler<TCommand, TEntity, TKey> : EntityCommandValidationHandler<TCommand, TEntity, TKey>
        where TCommand : IEntityCommand<TKey>, IValidatableCommand where TEntity : class, IEntity<TKey>
    {
        protected DeleteEntityCommandValidationHandler(IRepository repository)
            : base(repository)
        {
        }

        protected override void BuildValidator(ValidatorFactory factory, TCommand command)
        {
            factory.Add(new EntityExistValidator<TEntity, TKey>(command.Id, this.Repository));
        }
    }
}