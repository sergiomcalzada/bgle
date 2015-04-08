using System;

using bgle.CQRS.EventHandler;
using bgle.CQRS.EventStore.Factory;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CastleWindsor.Installer.Event
{
    public class EventHandlersInstaller : IWindsorInstaller
    {
        private readonly Type commandEventHandlerType;

        public EventHandlersInstaller(Type commandEventHandlerType)
        {
            this.commandEventHandlerType = commandEventHandlerType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining(this.commandEventHandlerType).BasedOn(typeof(IEventHandler<>)).WithServiceBase().LifestyleTransient(),
                Component.For<IEventHandlerFactory>().AsFactory().LifestyleTransient());
        }
    }
}