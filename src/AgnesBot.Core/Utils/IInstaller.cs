using Autofac;

namespace AgnesBot.Core.Utils
{
    public interface IInstaller
    {
        void Install(ContainerBuilder builder);
    }
}