namespace bgle.CQRS.Command
{
    public interface IEntityCommand : ICommand
    {
        bool IsTransient { get; }
    }

    public interface IEntityCommand<out TKey> : ICommand<TKey>, IEntityCommand
    {
    }
}