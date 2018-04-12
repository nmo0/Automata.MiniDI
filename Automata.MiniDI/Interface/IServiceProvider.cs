using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI.Interface
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType);

        T GetService<T>(Func<T, bool> filter) where T : class;

        object GetService(Func<ServiceDescriptor, bool> filter);
    }
}