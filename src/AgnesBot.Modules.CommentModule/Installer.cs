using AgnesBot.Core.Utils;
using Autofac;

namespace AgnesBot.Modules.CommentModule
{
    public class Installer : IInstaller
    {
        public void Install(ContainerBuilder builder)
        {
            builder.RegisterType<CommentRepository>()
                .As<ICommentRepository>()
                .InstancePerDependency();
        }
    }
}