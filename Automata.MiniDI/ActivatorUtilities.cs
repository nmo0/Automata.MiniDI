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

        /// <summary>
        /// 查找所有的引用
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static IEnumerable<string> GetAssemblyPaths()
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var name = "*.dll";

            string[] dll = null;

            if (System.Threading.Thread.GetDomain().FriendlyName.IndexOf("W3SVC") > -1)
            {
                var theAssemblyPath = Assembly.GetExecutingAssembly().Location;

                var key = "assembly";

                currentDirectory = theAssemblyPath.Substring(0, theAssemblyPath.LastIndexOf(key) + key.Length);

                dll = System.IO.Directory.GetFiles(currentDirectory, name, System.IO.SearchOption.AllDirectories);
            }
            else
            {
                dll = System.IO.Directory.GetFiles(currentDirectory, name);
            }

            return dll;
        }

        /// <summary>
        /// 从当前线程的AppDomain获取所有Assembly的物理dll路径
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAssemblyPathFromAppDomain()
        {
            return GetAssemblyPathFromAppDomain(AppDomain.CurrentDomain);
        }

        /// <summary>
        /// 从指定AppDomain获取所有Assembly的物理dll路径
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAssemblyPathFromAppDomain(AppDomain appDomain)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var result = new List<string>();
            foreach (var item in assemblies)
            {
                try
                {
                    if (!item.IsDynamic)
                    {
                        result.Add(item.Location);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据DLL名称获取Assembly
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete]
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

        /// <summary>
        /// 从指定AppDomain获取指定dll名称(ScopeName)的Assembly
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyFromAppDomain(string name, AppDomain appDomain)
        {
            var assemblys = appDomain.GetAssemblies();
            foreach (var assembly in assemblys)
            {
                if (!assembly.IsDynamic && name.Equals(assembly.ManifestModule.ScopeName))
                {
                    return assembly;
                }
            }
            return null;
        }

        /// <summary>
        /// 从当前线程的AppDomain获取指定dll名称(ScopeName)的Assembly
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Assembly GetAssemblyFromAppDomain(string name)
        {
            return GetAssemblyFromAppDomain(name, AppDomain.CurrentDomain);
        }

        /// <summary>
        /// 根据接口类查找实现
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        [Obsolete]
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

        /// <summary>
        /// 根据指定接口类型从当前线程的应用程序域查找所有的实现类型
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static IList<Type> ScanTypeFromAppDomain(Type interfaceType)
        {
            return ScanTypeFromAppDomain(interfaceType, AppDomain.CurrentDomain);
        }

        /// <summary>
        /// 根据指定接口类型从指定的应用程序域查找所有的实现类型
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="appDomain"></param>
        /// <returns></returns>
        public static IList<Type> ScanTypeFromAppDomain(Type interfaceType, AppDomain appDomain)
        {
            var result = new List<Type>();
            var assemblies = appDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var item in assembly.GetTypes())
                {
                    if (interfaceType.IsAssignableFrom(item))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 根据接口类查找实现
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [Obsolete]
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
            var implementationType = ScanTypeFromAppDomain(interfaceType);

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

        /// <summary>
        /// 根据指定类型/接口类型 创建实例
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="type"></param>
        /// <param name="implementationType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstance(Interface.IServiceProvider serviceProvider, Type type, Type implementationType, params object[] args)
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

        /// <summary>
        /// 根据方法名称调用指定类型的指定方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="method">方法名</param>
        /// <param name="instance">实例（如果是静态类则为null）</param>
        /// <param name="genericType">泛型类型</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static object Invoke(Type type, string method, object instance, Type[] genericType, object[] param)
        {
            MethodInfo methodInfo = null;
            
            Type[] types = new Type[param.Length];

            for (int i = 0; i < param.Length; i++)
            {
                types[i] = param[i].GetType();
            }

            if (genericType != null && genericType.Length > 0)
            {
                methodInfo = type.GetMethod(method, types).MakeGenericMethod(genericType);
            }
            else
            {
                methodInfo = type.GetMethod(method, types);
            }

            return methodInfo.Invoke(instance, param);
        }

        /// <summary>
        /// 查找所有标识指定的特性类的方法
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> FindMethodByAttribute(Type targetType, Type attributeType)
        {
            var result = new List<MethodInfo>();
            var methods = targetType.GetMethods(BindingFlags.Public);
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute(attributeType);
                if (attribute != null)
                {
                    result.Add(method);
                }
            }

            return result;
        }
    }
}