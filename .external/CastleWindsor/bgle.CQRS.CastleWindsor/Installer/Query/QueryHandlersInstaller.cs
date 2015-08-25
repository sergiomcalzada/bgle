using System;

using bgle.CQRS.QueryDispatcher.Factory;
using bgle.CQRS.QueryHandler;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CQRS.CastleWindsor.Installer.Query
{
    public class QueryHandlersInstaller : IWindsorInstaller
    {
        private readonly Type queryHandlerType;

        public QueryHandlersInstaller(Type queryHandlerType)
        {
            this.queryHandlerType = queryHandlerType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining(this.queryHandlerType).BasedOn(typeof(IQueryHandler<,>)).WithServiceBase().LifestyleTransient(),
                Component.For<IQueryHandlerFactory>().AsFactory().LifestyleTransient());
        }
    }
}