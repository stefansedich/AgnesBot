using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using Autofac;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;

namespace AgnesBot.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== AgnesBot ==");

            SetupContainer();

            IoC.Resolve<BotRunner>().Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
        }

        private static void SetupContainer()
        {
            var container = new WindsorContainer();

            RegisterComponents(container);
            RegisterModules(container);
            RegisterRaven(container);

            IoC.Initialize(container);
        }

        private static void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<IConfigurationManager>().ImplementedBy<ConfigurationManager>());
            container.Register(Component.For<BotRunner>().ImplementedBy<BotRunner>());
            container.Register(Component.For<IIrcClient>().ImplementedBy<IrcClient>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<UnitOfWorkFactory>());
        }

        private static void RegisterRaven(IWindsorContainer container)
        {
            var store = new DocumentStore { Url = "http://localhost:8080" };
            store.Initialize();

            container.Register(Component.For<IDocumentStore>().Instance(store));
        }

        private static void RegisterModules(IWindsorContainer container)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assemblies = Directory.GetFiles(path, "AgnesBot.Modules.*.dll")
                .Select(Assembly.LoadFile);

            Console.WriteLine("Registering Modules");

            foreach (var assembly in assemblies)
                container.Register(
                    AllTypes
                        .FromAssembly(assembly)
                        .BasedOn<IModule>()
                        .WithService
                        .FirstInterface()
                    );
                    
            var installers = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => typeof (IWindsorInstaller).IsAssignableFrom(x))
                .Select(x => (IWindsorInstaller)Activator.CreateInstance(x));

            foreach (var installer in installers)
                container.Install(installer);
        }
    }
}
