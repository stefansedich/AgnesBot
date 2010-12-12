using System;
using AgnesBot.Modules.UrlAggregatorModule.Domain;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AgnesBot.Modules.UrlAggregatorModule
{
    public class UrlAggregatorModuleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IUrlRepository>().ImplementedBy<UrlRepository>());
        }
    }
}