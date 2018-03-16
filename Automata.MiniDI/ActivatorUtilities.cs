using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Automata.MiniDI
{
    public class ActivatorUtilities
    {
        public static object CreateInstance(Type type, params object[] args)
        {
            var instance = Activator.CreateInstance(type, args);
            return instance;
        }

        private static IList<Type> ScanType(Type interfaceType)
        {
            var result = new List<Type>();
            var dll = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", System.IO.SearchOption.AllDirectories);
            var exe = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.exe", System.IO.SearchOption.AllDirectories);

            var allFile = dll.ToList();
            allFile.AddRange(exe.ToArray());

            foreach (var item in allFile)
            {
                result.AddRange(ScanType(interfaceType, item));
            }

            return result;
        }

        private static IList<Type> ScanType(Type interfaceType, string filePath)
        {
            var result = new List<Type>();

            Assembly assembly = Assembly.LoadFile(filePath);

            foreach (var item in assembly.GetTypes())
            {
                if (interfaceType.IsAssignableFrom(item))
                {
                    result.Add(item);
                }
            }

            return result;
        } 

        private static Type FindImplementationType(Type interfaceType)
        {
            var implementationType = ScanType(interfaceType);

            if (implementationType.Count == 0)
            {
                throw new Exception(string.Format("Can`t Find Implementation Of {0}", interfaceType.FullName));
            }

            if (implementationType.Count > 1)
            {
                throw new Exception(string.Format("Found multiple implementations of the interface {0}", interfaceType.FullName));
            }

            return implementationType.Single();
        }

        public static object CreateInstance(IServiceProvider serviceProvider, Type type, Type implementationType, params object[] args)
        {
            if (type.IsInterface && implementationType == null)
            {
                implementationType = FindImplementationType(type);
            }

            var constructors = implementationType.GetConstructors().OrderBy(m=>m.GetParameters().Length);

            foreach (var item in constructors)
            {
                var parameters = item.GetParameters();
                var flag = true;

                var ctorArgs = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];

                    if (args.Any(m => parameter.ParameterType.IsAssignableFrom(m.GetType())))
                    {
                        ctorArgs[i] = args.First(m => parameter.ParameterType.IsAssignableFrom(m.GetType()));

                        continue;
                    }

                    var serviceInstance = serviceProvider.GetService(parameter.ParameterType);

                    if (serviceInstance != null)
                    {
                        ctorArgs[i] = serviceInstance;

                        continue;
                    }

                    flag = false;

                    break;
                }

                if (flag)
                {
                    var instance = CreateInstance(implementationType, ctorArgs);
                    return instance;
                }
            }

            throw new MissingMethodException("创建" + implementationType.Name + "失败，因为没有找到合适的构造函数参数");
        }
    }
}