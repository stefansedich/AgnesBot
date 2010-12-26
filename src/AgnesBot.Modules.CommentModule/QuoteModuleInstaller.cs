using AgnesBot.Modules.QuoteModule.Domain;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AgnesBot.Modules.QuoteModule
{
    public class QuoteModuleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IQuoteRepository>().ImplementedBy<QuoteRepository>());
        }
    }
}