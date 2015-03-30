using System;

using bgle.Contracts.Repository;
using bgle.Contracts.Specifications;
using bgle.Contracts.Specifications.Entity;
using bgle.Entity;

namespace bgle.CQRS.CommandValidation
{
    public class EntityDoesNotExistValidator<TEntity> : BaseEntityValidator<TEntity>
        where TEntity : class, IEntity
    {
        private readonly Specification<TEntity> specification;

        public EntityDoesNotExistValidator(Specification<TEntity> specification, IRepository repository, string errorMessage, params string[] memberNames)
            : base(new Lazy<TEntity>(() => repository.Single(specification)), repository, errorMessage, memberNames)
        {
            this.specification = specification;
        }

        protected override bool IsValid()
        {
            return !this.Repository.Any(this.specification);
        }

        protected override bool IsValid(TEntity value)
        {
            return value == null;
        }
    }

    public class EntityDoesNotExistValidator<TEntity, TEntityType> : EntityDoesNotExistValidator<TEntity>
        where TEntity : class, IEntity<TEntityType>
    {
        public EntityDoesNotExistValidator(TEntityType id, IRepository repository)
            : base(new EntityByIdSpecification<TEntity, TEntityType>(id), repository, string.Format("{0} already exists", typeof(TEntity)), "Id")
        {
        }
    }
}