using System;
using System.Collections.Generic;
using System.Linq;
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

        public static object CreateInstance(IServiceProvider serviceProvider, Type type, params object[] args)
        {
            var constructors = type.GetConstructors().OrderBy(m=>m.GetParameters().Length);


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
                    var instance = CreateInstance(type, ctorArgs);
                    return instance;
                }
            }

            throw new MissingMethodException("创建" + type.Name + "失败，因为没有找到合适的构造函数参数");
        }
    }
}