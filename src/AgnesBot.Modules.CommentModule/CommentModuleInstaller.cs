using System;
using AgnesBot.Modules.CommentModule.Domain;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace AgnesBot.Modules.CommentModule
{
    public class CommentModuleInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICommentRepository>().ImplementedBy<CommentRepository>());
        }
    }
}