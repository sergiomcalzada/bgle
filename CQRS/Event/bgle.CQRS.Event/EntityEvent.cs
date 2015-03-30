namespace bgle.CQRS.Event
{
    public abstract class EntityEvent<TKey> : IEntityEvent<TKey>
    {
        public TKey Id { get; set; }
    }
}