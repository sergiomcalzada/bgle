using System.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command
{
    public class Command<T> : ICommand<T>
    {
        public Command()
            : this(default(T))
        {
        }

        public Command(T id)
        {
            this.Id = id;
        }

        [Required]
        public T Id { get; private set; }
    }
}