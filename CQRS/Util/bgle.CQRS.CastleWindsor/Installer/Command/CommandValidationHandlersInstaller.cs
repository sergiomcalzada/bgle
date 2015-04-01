using System;

using bgle.CQRS.CommandValidationHandler;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CastleWindsor.Installer.Command
{
    public class CommandValidationHandlersInstaller : IWindsorInstaller
    {
        private readonly Type commandValidationHandlerType;

        public CommandValidationHandlersInstaller(Type commandValidationHandlerType)
        {
            this.commandValidationHandlerType = commandValidationHandlerType;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining(this.commandValidationHandlerType).BasedOn(typeof(ICommandValidationHandler<>)).WithServiceBase().LifestyleTransient());
        }
    }
}