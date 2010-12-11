using AgnesBot.Modules.TinyUrlModule.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AgnesBot.Modules.TinyUrlModule
{
    public class TinyUrlModuleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ITinyUrlService>().ImplementedBy<TinyUrlService>());
        }
    }
}