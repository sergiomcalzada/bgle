namespace bgle.CQRS.Event
{
    public interface IEventPublisher
    {
        void Publish(IEvent entityEvent);
    }
}