using System.ComponentModel.DataAnnotations;

namespace bgle.CQRS.Command
{
    public abstract class Command<T> : ICommand<T>
    {
        protected Command()
            : this(default(T))
        {
        }

        protected Command(T id)
        {
            this.Id = id;
        }

        [Required]
        public T Id { get; private set; }
    }
}