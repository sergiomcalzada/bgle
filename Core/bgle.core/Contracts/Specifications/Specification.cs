using System;
using System.Linq;
using System.Linq.Expressions;

using bgle.Entity;

namespace bgle.Contracts.Specifications
{
    /// <summary>
    ///     http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    public class Specification<TEntity> : ISpecification<TEntity>
        where TEntity : IEntity
    {
        public Expression<Func<TEntity, bool>> Predicate;

        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            this.Predicate = predicate;
        }

        public IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(this.Predicate);
        }

        public Specification<TEntity> And(Specification<TEntity> specification)
        {
            return new Specification<TEntity>(this.Predicate.And(specification.Predicate));
        }

        public Specification<TEntity> And(Expression<Func<TEntity, bool>> predicate)
        {
            return new Specification<TEntity>(this.Predicate.And(predicate));
        }

        public static Specification<TEntity> operator &(Specification<TEntity> left, Specification<TEntity> right)
        {
            return left.And(right);
        }

        public Specification<TEntity> Or(Specification<TEntity> specification)
        {
            return new Specification<TEntity>(this.Predicate.Or(specification.Predicate));
        }

        public Specification<TEntity> Or(Expression<Func<TEntity, bool>> predicate)
        {
            return new Specification<TEntity>(this.Predicate.Or(predicate));
        }

        public static Specification<TEntity> operator |(Specification<TEntity> left, Specification<TEntity> right)
        {
            return left.Or(right);
        }

        public static bool operator true(Specification<TEntity> specification)
        {
            return false;
        }

        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }
    }
}