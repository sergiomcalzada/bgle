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

    public class EntityByIdQuery : EntityByIdQuery<string>
    {
        public EntityByIdQuery(string id)
            : base(id)
        {
        }
    }
}