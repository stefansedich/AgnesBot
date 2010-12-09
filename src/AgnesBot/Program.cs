using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AgnesBot.Core;
using AgnesBot.Domain.Interfaces;
using AgnesBot.Modules;
using AgnesBot.Repositories;
using Autofac;
using Raven.Client.Document;

namespace AgnesBot
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupContainer();

            IoC.Resolve<BotRunner>().Start();

            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
        }

        private static void SetupContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ConfigurationManager>()
                .As<IConfigurationManager>()
                .SingleInstance();

            builder.RegisterType<BotRunner>()
                .SingleInstance();

            builder.RegisterType<IrcClient>()
                .As<IIrcClient>()
                .SingleInstance();

            builder.RegisterType<CommentRepository>()
                .As<ICommentRepository>();

            RegisterRavenDBStore(builder);
            RegisterModulesAndHandlersInContainer(builder);
            
            var container = builder.Build();
            IoC.Initialize(container);
        }

        private static void RegisterRavenDBStore(ContainerBuilder builder)
        {
            var store = new DocumentStore { Url = "http://localhost:8080" };
            store.Initialize();
            builder.RegisterInstance(store);
        }

        private static void RegisterModulesAndHandlersInContainer(ContainerBuilder builder)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assemblies = Directory.GetFiles(path, "*.dll")
                .Select(assembly => Assembly.LoadFile(assembly))
                .Concat(new List<Assembly> { Assembly.GetExecutingAssembly() });

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(x => typeof (BaseModule).IsAssignableFrom(x) && !x.IsAbstract)
                .As<BaseModule>();
        }
    }
}
