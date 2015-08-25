using bgle.CQRS.CastleWindsor.Scope;
using bgle.CQRS.CommandDispatcher.Factory;
using bgle.CQRS.CommandHandler;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace bgle.CQRS.CastleWindsor.Installer.Command
{
    public class CommandHandlersInstaller : IWindsorInstaller
    {

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IScopedCommandHandlerFactory>().AsFactory().LifestyleTransient(),
                               Component.For<ICommandScope>().ImplementedBy<CastleWindsorCommandScope>().LifestyleTransient(),
                               Classes.FromAssemblyInDirectory(new AssemblyFilter(".", "*.CommandHandler.*dll")).BasedOn(typeof(ICommandHandler<>)).WithServiceBase().LifestyleTransient());

        }
    }
}