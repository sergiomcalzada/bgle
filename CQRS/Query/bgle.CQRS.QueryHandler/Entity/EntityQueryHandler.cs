using System.Collections.Generic;
using System.Linq;

using bgle.Contracts.Repository;
using bgle.Contracts.Specifications;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;
using bgle.CQRS.QueryResultBuilder;
using bgle.CQRS.QuerySpecificationBuilder;
using bgle.Entity;

namespace bgle.CQRS.QueryHandler
{
    public abstract class EntityQueryHandler<TQuery, TQueryResult, TEntity> : BaseQueryHandler<TQuery, TQueryResult>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
        where TEntity : IEntity
    {
        protected readonly IRepository Repository;

        protected EntityQueryHandler(IRepository repository)
        {
            this.Repository = repository;
        }
    }

    public abstract class EntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder> : EntityQueryHandler<TQuery, TQueryResult, TEntity>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
        where TEntity : class, IEntity
        where TQuerySpecificationBuilder : IQuerySpecificationBuilder<TQuery, TEntity>, new()
    {
        protected ISpecification<TEntity> Specification;

        protected EntitySpecificationQueryHandler(IRepository repository)
            : base(repository)
        {
        }

        protected override void BeforeHandle(TQuery query)
        {
            this.Specification = new TQuerySpecificationBuilder().Build(query);
        }

    }
    
    public abstract class SingleEntitySpecificationMaterializerQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder, TQueryResultBuilder> :
        EntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
        where TEntity : class, IEntity
        where TQuerySpecificationBuilder : IQuerySpecificationBuilder<TQuery, TEntity>, new()
        where TQueryResultBuilder : IQueryResultBuilder<TQueryResult, TEntity>, new()
    {

        protected SingleEntitySpecificationMaterializerQueryHandler(IRepository repository)
            : base(repository)
        {
        }

        protected override TQueryResult DoHandle(TQuery query)
        {
            var entity = this.Repository.Where(this.Specification).Single();
            return this.Materialize(entity, query);
        }

        protected virtual TQueryResult Materialize(TEntity entity, TQuery query)
        {
            var result = new TQueryResultBuilder().Build(entity, query);
            return result;
        }
    }
    
    public abstract class MiltipleEntitySpecificationMaterializerQueryHandler<TQuery, TQueryResult, TQueryResultItem, TEntity, TQuerySpecificationBuilder, TQueryResultBuilder> :
        EntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder>
        where TQuery : IQuery
        where TQueryResult : IQueryResultList<TQueryResultItem>, new()
        where TQueryResultItem : IQueryResultListItem
        where TEntity : class, IEntity
        where TQuerySpecificationBuilder : IQuerySpecificationBuilder<TQuery, TEntity>, new()
        where TQueryResultBuilder : IQueryResultBuilder<TQueryResultItem, TEntity>, new()
    {

        protected MiltipleEntitySpecificationMaterializerQueryHandler(IRepository repository)
            : base(repository)
        {
        }

        protected override TQueryResult DoHandle(TQuery query)
        {
            var entities = this.Repository.Where(this.Specification);
            return this.Materialize(entities, query);
        }

        protected virtual TQueryResult Materialize(IEnumerable<TEntity> entities, TQuery query)
        {
            var builder = new TQueryResultBuilder();
            var result = new TQueryResult();
            result.AddRange(entities.Select(e => builder.Build(e, query)));
            return result;
        }
    }


}