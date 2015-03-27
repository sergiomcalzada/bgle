using System;

using bgle.Entity;

namespace bgle.Contracts.Specifications.Entity
{
    public class FromUpdatedDateTimeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class, IEntity, IUpdatedDate
    {
        public FromUpdatedDateTimeSpecification(DateTime dateTime)
            : base(entity => dateTime <= entity.UpdatedDate)
        {
        }
    }

    public class ToUpdatedDateTimeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class, IEntity, IUpdatedDate
    {
        public ToUpdatedDateTimeSpecification(DateTime dateTime)
            : base(entity => entity.UpdatedDate < dateTime)
        {
        }
    }

    public class UpdatedDateTimeRangeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class, IEntity, IUpdatedDate
    {
        public UpdatedDateTimeRangeSpecification(DateTime fromDateTime, DateTime toDateTime)
            : base(entity => fromDateTime <= entity.UpdatedDate && entity.UpdatedDate < toDateTime)
        {
        }
    }
}