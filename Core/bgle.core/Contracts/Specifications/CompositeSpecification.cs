using System.Linq;

using bgle.Entity;

namespace bgle.Contracts.Specifications
{
    /// <summary>
    ///     http://devlicio.us/blogs/jeff_perrin/archive/2006/12/13/the-specification-pattern.aspx
    /// </summary>
    public abstract class CompositeSpecification<TEntity> : ISpecification<TEntity>
        where TEntity : IEntity
    {
        protected readonly Specification<TEntity> LeftSide;

        protected readonly Specification<TEntity> RightSide;

        protected CompositeSpecification(Specification<TEntity> leftSide, Specification<TEntity> rightSide)
        {
            this.LeftSide = leftSide;
            this.RightSide = rightSide;
        }

        public abstract IQueryable<TEntity> SatisfyingEntitiesFrom(IQueryable<TEntity> query);
    }
}