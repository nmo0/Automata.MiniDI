using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI
{
    public class ServiceDescriptor
    {
        public ServiceLifetime Lifetime { get; }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }

        public object ImplementationInstance { get; }
        

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