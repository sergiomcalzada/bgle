using System.Linq;
using System.Linq.Expressions;

using bgle.Entity;

namespace bgle.Contracts.Specifications
{
    /// <summary>
    ///     http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    public class AndSpecification<TEntity> : CompositeSpecification<TEntity>
        where TEntity : IEntity
    {
        public AndSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
            : base(leftSide, rightSide)
        {
        }

        public override IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query)
        {
            return query.Where(this.LeftSide.Predicate.And(this.RightSide.Predicate));
        }
    }
}