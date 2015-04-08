namespace bgle.CQRS.Query
{
    public interface IEntityByIdQuery<out T> : IQuery
    {
        T Id { get; }
    }
}