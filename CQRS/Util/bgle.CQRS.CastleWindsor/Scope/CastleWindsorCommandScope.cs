using System;

using bgle.Contracts.UnitOfWork;
using bgle.CQRS.CommandHandler;

using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;

namespace bgle.CastleWindsor.Scope
{
    public class CastleWindsorCommandScope : ICommandScope
    {
        private readonly IKernel kernel;

        private IDisposable scope;

        public CastleWindsorCommandScope(IKernel kernel)
        {
            this.kernel = kernel;
            this.scope = kernel.BeginScope();
            this.UnitOfWork = kernel.Resolve<IUnitOfWork>();
        }

        public IUnitOfWork UnitOfWork { get; private set; }

        public void Dispose()
        {
            this.kernel.ReleaseComponent(this.UnitOfWork);
            this.UnitOfWork = null;

            this.scope.Dispose();
            this.kernel.ReleaseComponent(this.scope);
            this.scope = null;
        }
    }
}