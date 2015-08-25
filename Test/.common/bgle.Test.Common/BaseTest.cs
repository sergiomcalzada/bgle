using System.Data.Entity;

using bgle.Contracts.DateTimeHandling;
using bgle.Contracts.Repository;
using bgle.CQRS.CastleWindsor.Installer;
using bgle.CQRS.CastleWindsor.Installer.Command;
using bgle.CQRS.CommandDispatcher;
using bgle.CQRS.EventStore;
using bgle.Test.Domain.Context;

using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using TestStack.BDDfy;
using TestStack.BDDfy.Configuration;

using Xunit.Abstractions;

namespace bgle.Test.Common
{
    
    public class BaseTest
    {
        protected readonly ITestOutputHelper OutputHelper;

        protected readonly WindsorContainer Container;

        public BaseTest(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
            this.Container = new WindsorContainer();
            this.InitializeIoC();
            Configurator.Processors.Add(() => new XunitReporter(this.OutputHelper));
        }

        public void InitializeIoC()
        {

            this.Container.AddFacility<TypedFactoryFacility>();

            this.Container.Register(Component.For<IDateTimeProvider, UtcDateTimeProvider>().LifestyleSingleton());
            this.RegisterDatabase();

            this.RegisterCommandConfiguration();
            this.Container.Register(Component.For<IEventStore, EmptyEventStore>().LifestyleSingleton());

            this.RegisterScenarios();

        }

        protected void RegisterCommandConfiguration()
        {
            /* Commands configuration*/
            this.Container.Register(Component.For<ICommandDispatcher, TransactionalCommandDispatcher>().LifestyleTransient());
            this.Container.Install(new CommandHandlersInstaller());
            this.Container.Install(new CommandValidationHandlersInstaller());
        }

        protected void RegisterDatabase()
        {
            /*Database access configuration*/
            this.Container.Install(new DatabaseAcessInstaller(typeof(TestDomainContext)));
        }

        protected void RegisterScenarios()
        {
            /*Test scenarios configuration*/
            this.Container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter("","*.Test.*dll")).BasedOn<ScenarioBase>().LifestyleTransient());
        }

        protected void RunScenario<TScenario>() where TScenario : IScenario
        {
            var context = ((DbContext)this.Container.Resolve<IDbContext>());
            if (context.Database.Exists()) context.Database.Delete();
            this.Container.Resolve<TScenario>().BDDfy();
            if (context.Database.Exists()) context.Database.Delete();
        }
    }
}

