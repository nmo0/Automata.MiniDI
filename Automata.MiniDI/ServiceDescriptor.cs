using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI
{
    public class ServiceDescriptor
    {
        public ServiceLifetime Lifetime { get; set; }

        /// <summary>
        /// 类型(包含接口)
        /// </summary>
        public Type ServiceType { get; set; }

        /// <summary>
        /// 实现类型
        /// </summary>
        public Type ImplementationType { get; set; }

        /// <summary>
        /// 实例
        /// </summary>
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