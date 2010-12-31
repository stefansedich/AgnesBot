using AgnesBot.Core.IrcUtils;
using AgnesBot.Core.UnitOfWork;
using AgnesBot.Core.Utils;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AgnesBot.Server.Installers
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IConfigurationManager>().ImplementedBy<ConfigurationManager>());
            container.Register(Component.For<BotRunner>().ImplementedBy<BotRunner>());
            container.Register(Component.For<IIrcClient>().ImplementedBy<IrcClient>());
            container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<UnitOfWorkFactory>());
        }
    }
}