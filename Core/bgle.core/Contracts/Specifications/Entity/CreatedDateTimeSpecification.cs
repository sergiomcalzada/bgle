using System;

using bgle.Entity;

namespace bgle.Contracts.Specifications.Entity
{
    public class FromCreatedDateTimeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class, IEntity, ICreatedDate
    {
        public FromCreatedDateTimeSpecification(DateTime dateTime)
            : base(entity => dateTime <= entity.CreatedDate)
        {
        }
    }

    public class ToCreatedDateTimeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class, IEntity, ICreatedDate
    {
        public ToCreatedDateTimeSpecification(DateTime dateTime)
            : base(entity => entity.CreatedDate < dateTime)
        {
        }
    }

    public class CreatedDateTimeRangeSpecification<TEntity> : Specification<TEntity>
        where TEntity : class, IEntity, ICreatedDate
    {
        public CreatedDateTimeRangeSpecification(DateTime fromDateTime, DateTime toDateTime)
            : base(entity => fromDateTime <= entity.CreatedDate && entity.CreatedDate < toDateTime)
        {
        }
    }
}