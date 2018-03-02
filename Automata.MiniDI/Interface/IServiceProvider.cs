using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI.Interface
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType);
    }
}