using System;
using AgnesBot.Core.Modules;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace AgnesBot.Server.Installers
{
    public class ModuleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Console.WriteLine("- Registering Modules -");

            RegisterModules(container); 
            ExecuteModuleInstallers(container);

            Console.WriteLine("- Modules Registered -");
        }

        private static void RegisterModules(IWindsorContainer container)
        {
            container.Register(
                AllTypes.FromAssemblyInDirectory(Program.ModuleAssemblyFilter)
                    .BasedOn<IModule>().WithService.FirstInterface()
                    .Where(x => x.Name.EndsWith("Service")).WithService.FirstInterface()
                    .Where(x => x.Name.EndsWith("Repository")).WithService.FirstInterface()
                );
        }

        private static void ExecuteModuleInstallers(IWindsorContainer container)
        {
            container.Install(
                FromAssembly.InDirectory(Program.ModuleAssemblyFilter)
                );
        }
    }
}