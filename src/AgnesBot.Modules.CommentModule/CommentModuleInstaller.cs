using AgnesBot.Core.Utils;
using AgnesBot.Modules.CommentModule.Domain;
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