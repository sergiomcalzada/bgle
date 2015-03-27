using bgle.Contracts.IdGenerator;

namespace bgle.CQRS.Command
{
    public class StringEntityCommand : EntityCommand<string>
    {
        public StringEntityCommand()
            : base(UidGenerator.NewStringUid(), true)
        {
        }

        public StringEntityCommand(string id)
            : base(id)
        {
        }
    }
}