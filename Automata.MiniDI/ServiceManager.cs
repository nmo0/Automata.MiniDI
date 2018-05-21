using Automata.MiniDI;
using Automata.MiniDI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automata.MiniDI
{
    /// <summary>
    /// Service Manager
    /// </summary>
    public class ServiceManager
    {
        public static Interface.IServiceProvider ServiceProvider;

        /// <summary>
        /// Build Service
        /// </summary>
        /// <param name="build"></param>
        public static void Build(Action<ServiceCollection> build)
        {
            var serviceCollection = new ServiceCollection();

            build(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public static T GetService<T>(Func<T, bool> filter) where T : class
        {
            return ServiceProvider.GetService(filter);
        }
    }
}
