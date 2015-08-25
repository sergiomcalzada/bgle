using bgle.CQRS.CommandValidationHandler;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CQRS.CastleWindsor.Installer.Command
{
    public class CommandValidationHandlersInstaller : IWindsorInstaller
    {
       

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(".", "*.CommandValidationHandler.*dll")).BasedOn(typeof(ICommandValidationHandler<>)).WithServiceBase().LifestyleTransient());    
        }
    }
}