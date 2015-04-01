using System;

using bgle.CastleWindsor.Scope;
using bgle.CQRS.CommandBus;
using bgle.CQRS.CommandHandler;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CastleWindsor.Installer.Command
{
    public class CommandHandlersInstaller : IWindsorInstaller
    {
        private readonly Type commandValidationHandlerType;

        public CommandHandlersInstaller(Type commandValidationHandlerType)
        {
            this.commandValidationHandlerType = commandValidationHandlerType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining(this.commandValidationHandlerType).BasedOn(typeof(ICommandHandler<>)).WithServiceBase().LifestyleTransient(),
                Component.For<IScopedCommandHandlerFactory>().AsFactory().LifestyleTransient(),
                Component.For<ICommandScope>().ImplementedBy<CastleWindsorCommandScope>().LifestyleTransient());
        }
    }
}