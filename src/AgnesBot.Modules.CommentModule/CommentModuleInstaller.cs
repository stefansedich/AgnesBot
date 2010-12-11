using AgnesBot.Core.Utils;
using Autofac;

namespace AgnesBot.Modules.CommentModule
{
    public class CommentModuleInstaller : IInstaller
    {
        public void Install(ContainerBuilder builder)
        {
            builder.RegisterType<CommentRepository>()
                .As<ICommentRepository>()
                .SingleInstance();
        }
    }
}