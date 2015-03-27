namespace bgle.CQRS.Command
{
    public interface ICommand
    {
    }

    public interface ICommand<out T>
    {
        T Id { get; }
    }
}