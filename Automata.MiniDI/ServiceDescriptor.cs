using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI
{
    public class ServiceDescriptor
    {
        public ServiceLifetime Lifetime { get; set; }

        public Type ServiceType { get; set; }

        public Type ImplementationType { get; set; }

        public object ImplementationInstance { get; set; }
        

        public ServiceDescriptor(
            Type serviceType,
            Type implementationType,
            object implementationInstance,
            ServiceLifetime lifetime)
            : this(serviceType, lifetime)
        {
            ImplementationInstance = implementationInstance;
            ImplementationType = implementationType;
        }

        private ServiceDescriptor(Type serviceType, ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
            ServiceType = serviceType;
        }
    }
}