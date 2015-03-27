using System.Linq.Expressions;

using bgle.Entity;

namespace bgle.Contracts.Specifications.Entity
{
    public class EntityByIdSpecification<TEntity, TKey> : Specification<TEntity>
        where TEntity : class, IEntity, IEntity<TKey>
    {
        public EntityByIdSpecification(TKey id)
            : base(ExpressionExtenions.Equals<TEntity, TKey>(ExpressionExtenions.GetPropertyName<TEntity, TKey>(e => e.Id), id))
        {
        }
    }
}