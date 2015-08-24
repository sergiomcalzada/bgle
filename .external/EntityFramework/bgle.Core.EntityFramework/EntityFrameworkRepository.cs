using System.Data.Entity;
using System.Linq;

using bgle.Contracts.Repository;
using bgle.Contracts.Specifications;
using bgle.Contracts.UnitOfWork;
using bgle.Entity;

namespace bgle.EntityFramework
{
    public class EntityFrameworkRepository : IRepository
    {
        private readonly DbContext dbContext;

        public EntityFrameworkRepository(IUnitOfWork unitOfWork, IDbContext dbContext)
        {
            this.dbContext = (DbContext)dbContext;
            this.UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; private set; }

        public TEntity Create<TEntity>() where TEntity : class, IEntity
        {
            return this.GetSet<TEntity>().Create();
        }

        public TEntity Find<TEntity>(params object[] keyValues) where TEntity : class, IEntity
        {
            return this.GetSet<TEntity>().Find(keyValues);
        }

        public TEntity Add<TEntity>(TEntity obj) where TEntity : class, IEntity
        {
            return this.GetSet<TEntity>().Add(obj);
        }

        public void Remove<TEntity>(TEntity obj) where TEntity : class, IEntity
        {
            this.GetSet<TEntity>().Remove(obj);
        }

        public int Count<TEntity>() where TEntity : class, IEntity
        {
            return this.GetSet<TEntity>().Count();
        }

        public TEntity Single<TEntity>(ISpecification<TEntity> criteria) where TEntity : class, IEntity
        {
            return this.Where(criteria).Single();
        }

        public bool Any<TEntity>(ISpecification<TEntity> specification) where TEntity : class, IEntity
        {
            return this.Where(specification).Any();
        }

        public IQueryable<TEntity> Where<TEntity>(ISpecification<TEntity> specification) where TEntity : class, IEntity
        {
            return specification.SatisfyingEntitiesFrom(this.GetSet<TEntity>());
        }

        private IDbSet<TEntity> GetSet<TEntity>() where TEntity : class, IEntity
        {
            return this.dbContext.Set<TEntity>();
        }
    }
}