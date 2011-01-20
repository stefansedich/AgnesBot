using System.Collections.Generic;
using Castle.Windsor;

namespace AgnesBot.Core.Utils
{
    public static class IoC
    {
        private static IWindsorContainer _container;

        public static void Initialize(IWindsorContainer container)
        {
            _container = container;
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }
    }
}