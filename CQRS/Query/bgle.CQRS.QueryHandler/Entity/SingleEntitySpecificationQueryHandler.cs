using System.Linq;

using bgle.Contracts.Repository;
using bgle.CQRS.Query;
using bgle.CQRS.QueryResult;
using bgle.CQRS.QueryResultBuilder;
using bgle.CQRS.QuerySpecificationBuilder;
using bgle.Entity;

namespace bgle.CQRS.QueryHandler
{
    public abstract class SingleEntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder, TQueryResultBuilder> :
        EntitySpecificationQueryHandler<TQuery, TQueryResult, TEntity, TQuerySpecificationBuilder>
        where TQuery : IQuery
        where TQueryResult : IQueryResult
        where TEntity : class, IEntity
        where TQuerySpecificationBuilder : IQuerySpecificationBuilder<TQuery, TEntity>, new()
        where TQueryResultBuilder : IQueryResultBuilder<TQueryResult, TEntity>, new()
    {

        protected SingleEntitySpecificationQueryHandler(IRepository repository)
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
}