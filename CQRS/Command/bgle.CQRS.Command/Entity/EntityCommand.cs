namespace bgle.CQRS.Command
{
    public abstract class EntityCommand<TKey> : ValidatableCommand<TKey>, IEntityCommand<TKey>
    {
        protected EntityCommand()
            : this(default(TKey), true)
        {
        }

        protected EntityCommand(TKey id)
            : this(id, false)
        {
        }

        protected EntityCommand(TKey id, bool isTransient)
            : base(id)
        {
            this.IsTransient = isTransient;
        }

        public bool IsTransient { get; private set; }
    }
}