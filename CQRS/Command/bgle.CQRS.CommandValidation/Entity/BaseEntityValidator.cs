using System;

using bgle.Contracts.Repository;
using bgle.Entity;

namespace bgle.CQRS.CommandValidation
{
    public abstract class BaseEntityValidator<TEntity> : BaseValidator
        where TEntity : class, IEntity
    {
        protected readonly IRepository Repository;

        private readonly Lazy<TEntity> lazyEntity;

        protected BaseEntityValidator(Lazy<TEntity> lazyEntity, IRepository repository, string errorMessage, params string[] memberNames)
            : base(errorMessage, memberNames)
        {
            this.lazyEntity = lazyEntity;
            this.Repository = repository;
        }

        protected override bool IsValid()
        {
            var entity = this.lazyEntity.Value;

            return entity != null && this.IsValid(this.lazyEntity.Value);
        }

        protected abstract bool IsValid(TEntity value);
    }
}