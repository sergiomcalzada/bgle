
using bgle.Contracts.Repository;
using bgle.CQRS.CommandDispatcher;
using bgle.Test.Common;

using TestStack.BDDfy;

using Xunit;
using Xunit.Abstractions;

namespace bgle.CQRS.Command.Identity.Test
{
    [Story(
        AsA = "As an new user",
        IWant = "I want to register",
        SoThat = "An user ir created in the system")]
    public class UserIsRegisteredInTheSystemStory : BaseTest
    {
        public UserIsRegisteredInTheSystemStory(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void UserIsRegisteredInTheSystemScenario()
        {
            this.RunScenario<NewUserRegisterInTheSystemScenario>();
        }
    }

    public class NewUserRegisterInTheSystemScenario : ScenarioBase, IScenario
    {
        private string userId;

        public NewUserRegisterInTheSystemScenario(ICommandDispatcher commandDispatcher, IRepository repository)
            : base(commandDispatcher, repository)
        {
        }

        private void GivenAnUsernameAndPassword() { }

        private void WhenTheUserIsRegistered()
        {
            var cmd = new CreateUserCommand("UserName", "PasswordHash", "email");
            this.userId = cmd.Id;
            var result = this.CommandDispatcher.Dispatch(cmd);
            Assert.True(result.IsValid);
        }

        private void ThenTheUserExits() { }

    }
}