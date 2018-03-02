using Automata.MiniDI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI.Extensions
{
    public static class ServiceCollectionContainerBuilderExtensions
    {
        public static ServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            return new ServiceProvider(services);
        }
    }
}