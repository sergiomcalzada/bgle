using System.Collections.Generic;
using System.Linq;

using bgle.Contracts.Repository;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;
using bgle.CQRS.QueryResultBuilder;
using bgle.CQRS.QuerySpecificationBuilder;
using bgle.Entity;

namespace bgle.CQRS.QueryHandler
{
    public abstract class MiltipleEntitySpecificationQueryHandler<TQuery, TQueryResult, TQueryResultItem, TEntity, TQuerySpecificationBuilder, TQueryResultBuilder> :
        EntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder>
        where TQuery : IQuery
        where TQueryResult : IQueryResultCollection<TQueryResultItem>, new()
        where TQueryResultItem : IQueryResultCollectionItem
        where TEntity : class, IEntity
        where TQuerySpecificationBuilder : IQuerySpecificationBuilder<TQuery, TEntity>, new()
        where TQueryResultBuilder : IQueryResultBuilder<TQueryResultItem, TEntity>, new()
    {

        protected MiltipleEntitySpecificationQueryHandler(IRepository repository)
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