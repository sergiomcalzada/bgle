namespace bgle.CQRS.Event
{
    public class EntityEvent<TKey> : IEntityEvent<TKey>
    {
        public TKey Id { get; set; }
    }
}