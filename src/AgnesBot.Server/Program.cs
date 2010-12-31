using System;
using System.IO;
using System.Reflection;
using AgnesBot.Core.Utils;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace AgnesBot.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("- Starting AgnesBot -");

            SetupContainer();
            IoC.Resolve<BotRunner>().Start();    
            
            Console.WriteLine("- AgnesBot Started -");
        }

        private static void SetupContainer()
        {
            var container = new WindsorContainer();

            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, false));
            container.Install(FromAssembly.This());

            IoC.Initialize(container);
        }
    }
}
