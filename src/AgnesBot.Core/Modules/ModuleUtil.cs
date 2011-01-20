using System.IO;
using System.Reflection;
using Castle.MicroKernel.Registration;

namespace AgnesBot.Core.Modules
{
    public static class ModuleUtil
    {
        public static AssemblyFilter ModuleAssemblyFilter
        {
            get
            {
                string moduleFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                return new AssemblyFilter(moduleFolder, "AgnesBot.Modules.*");
            }
        }
    }
}