using Automata.MiniDI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automata.MiniDI.Extensions
{
    public static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddTransient(
            this IServiceCollection services,
            Type serviceType)
        {
            Type implementationType = null;

            if (!serviceType.IsInterface)
            {
                implementationType = serviceType;
            }

            return services.AddTransient(serviceType, implementationType);
        }

        public static IServiceCollection AddTransient(
            this IServiceCollection services,
            Type serviceType,
            Type implementationType)
        {
            return Add(services, serviceType, implementationType, null, ServiceLifetime.Transient);
        }

        public static IServiceCollection AddTransient<TService>(
            this IServiceCollection services)
        {
            return services.AddTransient(typeof(TService));
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(
            this IServiceCollection services)
        {
            return services.AddTransient(typeof(TService), typeof(TImplementation));
        }

        public static IServiceCollection AddSingleton(
            this IServiceCollection services,
            object implementationInstance)
        {
            var serviceType = implementationInstance.GetType();
            return services.AddSingleton(serviceType, serviceType, implementationInstance);
        }

        public static IServiceCollection AddSingleton(
            this IServiceCollection services,
            Type serviceType,
            object implementationInstance)
        {
            return services.AddSingleton(serviceType, null, implementationInstance);
        }

        public static IServiceCollection AddSingleton(
            this IServiceCollection services,
            Type serviceType,
            Type implementationType)
        {
            return Add(services, serviceType, implementationType, null, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddSingleton(
            this IServiceCollection services,
            Type serviceType,
            Type implementationType,
            object implementationInstance)
        {
            return Add(services, serviceType, implementationType, implementationInstance, ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddSingleton<TService>(
            this IServiceCollection services)
        {
            Type implementationType = null;

            if (!typeof(TService).IsInterface)
            {
                implementationType = typeof(TService);
            }

            return services.AddSingleton(typeof(TService), implementationType);
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(
            this IServiceCollection services)
        {
            return services.AddSingleton(typeof(TService), typeof(TImplementation), null);
        }

        private static IServiceCollection Add(
            IServiceCollection collection,
            Type serviceType,
            Type implementationType,
            object implementationInstance,
            ServiceLifetime lifetime)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, implementationInstance, lifetime);
            collection.Add(descriptor);
            return collection;
        }
    }
}