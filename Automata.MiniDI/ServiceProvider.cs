using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI
{
    public class ServiceProvider : Interface.IServiceProvider
    {
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();
        private IEnumerable<ServiceDescriptor> _serviceDescriptors;

        public ServiceProvider(IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
        }

        public object GetService(Type serviceType)
        {
            lock (_cache)
            {
                if (_cache.ContainsKey(serviceType))
                {
                    return _cache[serviceType];
                }

                return CreateInstance(serviceType);
            }
        }

        /// <summary>
        /// 根据过滤器查找依赖项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public object GetService(Func<ServiceDescriptor, bool> filter)
        {
            foreach (var item in _serviceDescriptors)
            {
                if (filter(item))
                {
                    return GetService(item.ImplementationType);
                }
            }

            return null;
        }

        /// <summary>
        /// 根据过滤器查找依赖项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T GetService<T>(Func<T, bool> filter) where T : class
        {
            var serviceType = typeof(T);

            foreach (var item in _serviceDescriptors)
            {
                if (item.ServiceType == serviceType || serviceType.IsAssignableFrom(item.ServiceType))
                {
                    var service = (T)GetService(typeof(T));

                    if (filter(service))
                    {
                        return service;
                    }
                }
            }
            return null;
        }

        private object CreateInstance(Type serviceType)
        {
            ServiceDescriptor serviceDescriptor = null;
            if (!serviceType.IsInterface && _serviceDescriptors.Any(m => serviceType.IsAssignableFrom(m.ImplementationType)))
            {
                serviceDescriptor = _serviceDescriptors.First(m => serviceType.IsAssignableFrom(m.ImplementationType));
            }
            else if (serviceType.IsInterface)
            {
                serviceDescriptor = _serviceDescriptors.First(m => serviceType.FullName.Equals(m.ServiceType.FullName));
            }

            object serviceInstance = null;

            if (serviceDescriptor.ImplementationInstance == null || serviceDescriptor.Lifetime == ServiceLifetime.Transient)
            {
                serviceInstance = ActivatorUtilities.CreateInstance(this, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType);
            }
            else
            {
                serviceInstance = serviceDescriptor.ImplementationInstance;
            }

            if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton)
            {
                _cache.Add(serviceType, serviceInstance);
            }

            return serviceInstance;

            //throw new Exception(nameof(serviceType) + " 不存在");
        }
    }
}