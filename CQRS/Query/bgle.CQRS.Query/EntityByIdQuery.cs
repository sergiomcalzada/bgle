namespace bgle.CQRS.Query
{
    public abstract class EntityByIdQuery<T> : IEntityByIdQuery<T>
    {
        protected EntityByIdQuery(T id)
        {
            this.Id = id;
        }

        public T Id { get; private set; }
    }

    public class EntityByIdStringQuery : EntityByIdQuery<string>
    {
        public EntityByIdStringQuery(string id)
            : base(id)
        {
        }
    }
}