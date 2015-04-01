using System.Collections.Concurrent;

using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle.Scoped;

namespace bgle.CastleWindsor.Lifestile
{
    public class LifetimeScopeOrTransientAccessor : IScopeAccessor
    {
        private readonly ConcurrentStack<ILifetimeScope> lifetimeScopeCache;

        public LifetimeScopeOrTransientAccessor()
        {
            this.lifetimeScopeCache = new ConcurrentStack<ILifetimeScope>();
        }

        public void Dispose()
        {
            var scope = CallContextLifetimeScope.ObtainCurrentScope();
            if (scope != null)
            {
                scope.Dispose();
            }
            foreach (var lifetimeScope in this.lifetimeScopeCache.ToArray())
            {
                lifetimeScope.Dispose();
            }
        }

        public ILifetimeScope GetScope(CreationContext context)
        {
            var scope = (ILifetimeScope)CallContextLifetimeScope.ObtainCurrentScope();
            if (scope != null)
            {
                return scope;
            }

            scope = new DefaultLifetimeScope();
            this.lifetimeScopeCache.Push(scope);
            return scope;
        }
    }
}