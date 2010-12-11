using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.Modules;
using AgnesBot.Core.Utils;
using Autofac;
using Raven.Client.Document;

namespace AgnesBot.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupContainer();

            Console.WriteLine("== AgnesBot ==");

            IoC.Resolve<BotRunner>().Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
        }

        private static void SetupContainer()
        {
            var builder = new ContainerBuilder();

            RegisterComponents(builder);
            RegisterModules(builder);
            RegisterRaven(builder);
            
            IoC.Initialize(builder.Build());
        }

        private static void RegisterComponents(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationManager>()
                .As<IConfigurationManager>()
                .SingleInstance();

            builder.RegisterType<BotRunner>()
                .SingleInstance();

            builder.RegisterType<IrcClient>()
                .As<IIrcClient>()
                .SingleInstance();
        }

        private static void RegisterRaven(ContainerBuilder builder)
        {
            var store = new DocumentStore { Url = "http://localhost:8080" };
            store.Initialize();
            builder.RegisterInstance(store);
        }

        private static void RegisterModules(ContainerBuilder builder)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assemblies = Directory.GetFiles(path, "AgnesBot.Modules.*.dll")
                .Select(Assembly.LoadFile);

            Console.WriteLine("Registering Modules");

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(x => typeof (IModule).IsAssignableFrom(x) && !x.IsAbstract)
                .OnRegistered(x => Console.WriteLine(string.Format("{0} Registered", x.ComponentRegistration.Activator)))
                .As<IModule>();
            
            var installers = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => typeof (IInstaller).IsAssignableFrom(x))
                .Select(x => (IInstaller) Activator.CreateInstance(x));

            foreach (var installer in installers)
                installer.Install(builder);
        }
    }
}
