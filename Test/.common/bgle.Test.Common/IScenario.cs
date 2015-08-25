using bgle.Contracts.Repository;
using bgle.CQRS.CommandDispatcher;

namespace bgle.Test.Common
{
    public interface IScenario
    {
        ICommandDispatcher CommandDispatcher { get; }

        IRepository Repository { get; }
    }
}