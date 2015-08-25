using bgle.Contracts.Repository;
using bgle.CQRS.CommandDispatcher;

namespace bgle.Test.Common
{
    public class ScenarioBase : IScenario
    {
        public ICommandDispatcher CommandDispatcher { get; private set; }

        public IRepository Repository { get; private set; }

        public ScenarioBase(ICommandDispatcher commandDispatcher, IRepository repository)
        {
            this.CommandDispatcher = commandDispatcher;
            this.Repository = repository;
        }
    }
}