namespace bgle.CQRS.Command.Identity
{
    public class DeleteUserCommand : StringEntityCommand
    {
        public DeleteUserCommand(string id)
            : base(id)
        {
        }
    }
}