namespace bgle.CQRS.Event
{
    public interface IEntityEvent<TKey> : IEvent
    {
        TKey Id { get; set; }
    }
}