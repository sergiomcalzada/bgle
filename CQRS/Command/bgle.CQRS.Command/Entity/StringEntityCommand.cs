using bgle.Contracts.IdGenerator;

namespace bgle.CQRS.Command
{
    public abstract class StringEntityCommand : EntityCommand<string>
    {
        protected StringEntityCommand()
            : base(UidGenerator.NewStringUid(), true)
        {
        }

        protected StringEntityCommand(string id)
            : base(id)
        {
        }
    }
}