namespace bgle.DomainEntity
{
    public interface IEntity
    {
    }

    public interface IEntity<TKey> : IEntity, ICreatedDate, IUpdatedDate
    {
        TKey Id { get; set; }
    }
}