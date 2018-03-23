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

        public static Assembly GetAssembly(string name)
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            IEnumerable<string> dll = null;

            if (System.Threading.Thread.GetDomain().FriendlyName.IndexOf("W3SVC") > -1)
            {
                var theAssemblyPath = Assembly.GetExecutingAssembly().Location;

                var key = "assembly";

                currentDirectory = theAssemblyPath.Substring(0, theAssemblyPath.LastIndexOf(key) + key.Length);

                dll = System.IO.Directory.GetFiles(currentDirectory, name, System.IO.SearchOption.AllDirectories).ToList();
            }
            else
            {
                dll = System.IO.Directory.GetFiles(currentDirectory, name).ToList();
            }

            var file = dll.FirstOrDefault();
            if (file != null)
            {
                return Assembly.LoadFile(file);
            }

            return null;
        }

        private static IList<Type> ScanType(Type interfaceType)
        {
            var result = new List<Type>();

            var allFile = new List<string>();

            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (System.Threading.Thread.GetDomain().FriendlyName.IndexOf("W3SVC") > -1)
            {
                var theAssemblyPath = Assembly.GetExecutingAssembly().Location;

                var key = "assembly";

                currentDirectory = theAssemblyPath.Substring(0, theAssemblyPath.LastIndexOf(key) + key.Length);

                var dll = System.IO.Directory.GetFiles(currentDirectory, "*.dll", System.IO.SearchOption.AllDirectories);
                var exe = System.IO.Directory.GetFiles(currentDirectory, "*.exe", System.IO.SearchOption.AllDirectories);

                allFile = dll.ToList();
                allFile.AddRange(exe.ToArray());

                //currentDirectory = System.IO.Path.Combine(currentDirectory, "bin");
            }
            else
            {
                var dll = System.IO.Directory.GetFiles(currentDirectory, "*.dll");
                var exe = System.IO.Directory.GetFiles(currentDirectory, "*.exe");

                allFile = dll.ToList();
                allFile.AddRange(exe.ToArray());
            }

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

            //忽略系统程序集
            if (System.Text.RegularExpressions.Regex.IsMatch(assembly.FullName, @"^System."))
            {
                return result;
            }

            foreach (var item in assembly.GetTypes())
            {
                //if (interfaceType.IsAssignableFrom(item) ||
                //    Array.Exists(item.GetInterfaces(), t => interfaceType.FullName.Equals(t.FullName)) ||
                //    Array.Exists(item.GetInterfaces(), t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceType))
                //{
                //    result.Add(item);
                //}
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