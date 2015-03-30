namespace bgle.CQRS.Event
{
    public class EmptyEventPublisher : IEventPublisher
    {
        public void Publish(IEvent entityEvent)
        {
        }
    }
}