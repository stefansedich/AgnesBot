using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace AgnesBot.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("- Starting AgnesBot -");

            SetupContainer();

            IoC.Resolve<BotRunner>().Start();
        }

        private static void SetupContainer()
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, false));

            var moduleAssemblies = GetModuleAssemblies();

            RegisterComponents(container);
            RegisterModules(container, moduleAssemblies);
            RegisterRaven(container, moduleAssemblies);

            IoC.Initialize(container);
        }

        private static void RegisterComponents(IWindsorContainer container)
        {
            container.Register(Component.For<IConfigurationManager>().ImplementedBy<ConfigurationManager>());
            container.Register(Component.For<BotRunner>().ImplementedBy<BotRunner>());
            container.Register(Component.For<IIrcClient>().ImplementedBy<IrcClient>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<UnitOfWorkFactory>());
        }

        private static void RegisterRaven(IWindsorContainer container, IEnumerable<Assembly> moduleAssemblies)
        {
            var store = new DocumentStore { ConnectionStringName = "RavenDb" };
            store.Initialize();

            container.Register(Component.For<IDocumentStore>().Instance(store));
            
            foreach (var assembly in moduleAssemblies)
                IndexCreation.CreateIndexes(assembly, store);
        }

        private static void RegisterModules(IWindsorContainer container, IEnumerable<Assembly> moduleAssemblies)
        {
            Console.WriteLine("- Registering Modules -");

            foreach (var assembly in moduleAssemblies)
            {
                Console.WriteLine("Registering Module: " + assembly.FullName);

                container.Register(
                    AllTypes
                        .FromAssembly(assembly)
                        .BasedOn<IModule>()
                        .WithService
                        .FirstInterface()
                    );
            }

            var installers = moduleAssemblies.SelectMany(x => x.GetTypes())
                .Where(x => typeof (IWindsorInstaller).IsAssignableFrom(x))
                .Select(x => (IWindsorInstaller)Activator.CreateInstance(x));

            foreach (var installer in installers)
                container.Install(installer);

            Console.WriteLine("- Modules Registered -");
        }

        private static IList<Assembly> GetModuleAssemblies()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return Directory.GetFiles(path, "AgnesBot.Modules.*.dll")
                .Select(Assembly.LoadFile)
                .ToList();
        }
    }
}
