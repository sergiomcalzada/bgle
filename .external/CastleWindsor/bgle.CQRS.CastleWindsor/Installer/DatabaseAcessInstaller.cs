using System;

using bgle.CastleWindsor.Lifestile;
using bgle.Contracts.Repository;
using bgle.Contracts.UnitOfWork;
using bgle.EntityFramework;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CQRS.CastleWindsor.Installer
{
    public class DatabaseAcessInstaller : IWindsorInstaller
    {
        private readonly Type dbContextType;

        public DatabaseAcessInstaller(Type dbContextType)
        {
            this.dbContextType = dbContextType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component.For<IRepository, EntityFrameworkRepository>().LifestyleScoped<LifetimeScopeOrTransientAccessor>(),
                Component.For<IUnitOfWork, EntityFrameworkUnitOfWork>().LifestyleScoped<LifetimeScopeOrTransientAccessor>(),
                Component.For<IDbContext>().ImplementedBy(this.dbContextType).LifestyleScoped<LifetimeScopeOrTransientAccessor>());
        }
    }
}