using System;

using bgle.Contracts.UnitOfWork;

namespace bgle.CQRS.CommandHandler
{
    public interface ICommandScope : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}