using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AgnesBot.Core;
using AgnesBot.Modules;
using Autofac;
using Meebey.SmartIrc4net;

namespace AgnesBot
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupContainer();

            IoC.Resolve<AgnesBotRunner>().Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
        }

        private static void SetupContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ConfigurationManager>()
                .As<IConfigurationManager>()
                .SingleInstance();

            builder.RegisterType<AgnesBotRunner>()
                .SingleInstance();

            builder.RegisterType<IrcClient>()
                .SingleInstance();

            RegisterModulesAndHandlersInContainer(builder);
            
            var container = builder.Build();
            IoC.Initialize(container);
        }

        private static void RegisterModulesAndHandlersInContainer(ContainerBuilder builder)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assemblies = Directory.GetFiles(path, "*.dll")
                .Select(assembly => Assembly.LoadFile(assembly))
                .Concat(new List<Assembly> { Assembly.GetExecutingAssembly() });
            
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(x => typeof(BaseModule).IsAssignableFrom(x) && !x.IsAbstract)
                .AsSelf();
        }
    }
}
