using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI.Extensions
{
    public static class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }
    }
}