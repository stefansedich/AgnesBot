using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using ReflectionUtil = Castle.Core.Internal.ReflectionUtil;

namespace AgnesBot.Server.Installers
{
    public class RavenInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var documentStore = new DocumentStore { ConnectionStringName = "RavenDb" };
            documentStore.Initialize();

            container.Register(Component.For<IDocumentStore>().Instance(documentStore));

            CreateIndexesForModules(documentStore);
        }

        private static void CreateIndexesForModules(DocumentStore documentStore)
        {
            var assemblies = ReflectionUtil.GetAssemblies(Program.ModuleAssemblyFilter);

            foreach(var assembly in assemblies)
                IndexCreation.CreateIndexes(assembly, documentStore);
        }
    }
}