using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Automata.MiniDI.Extensions;
using Automata.MiniDI.Test.Services;

namespace Automata.MiniDI.Test
{
    [TestClass]
    public class ServiceTest
    {
        [TestMethod]
        public void GetServiceTest()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IFileService, FileService>();

            var provider = services.BuildServiceProvider();

            var fileService = provider.GetService<IFileService>();

            Assert.IsNotNull(fileService);
        }

        [TestMethod]
        public void GetSingletonServiceTest()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IFileService, FileService>();

            var provider = services.BuildServiceProvider();

            var fileService1 = provider.GetService<IFileService>();
            var fileService2 = provider.GetService<IFileService>();

            Assert.AreEqual(fileService1.guid, fileService2.guid);
        }

        [TestMethod]
        public void GetTransientServiceTest()
        {
            var services = new ServiceCollection();

            services.AddTransient<OtherService>();

            var provider = services.BuildServiceProvider();

            var otherService1 = provider.GetService<OtherService>();
            var otherService2 = provider.GetService<OtherService>();

            Assert.AreNotEqual(otherService1.guid, otherService2.guid);
        }



        [TestMethod]
        public void GetDependServiceTest()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<OtherService>();
            services.AddTransient<IMyService, MyService>();
            services.AddTransient<IMyService2, MyService2>();

            var provider = services.BuildServiceProvider();

            var myService2 = provider.GetService<IMyService2>();

            Assert.IsNotNull(myService2);
        }


        ///// <summary>
        ///// 没有指定实现类型也能查找到依赖
        ///// </summary>
        //[TestMethod]
        //public void GetServiceWithNoImplementation()
        //{
        //    //var services = new ServiceCollection();

        //    //services.AddSingleton<IFileService, FileService>();
        //    //services.AddSingleton<OtherService>();
        //    //services.AddTransient<IMyService, MyService>();
        //    //services.AddTransient<IMyService2, MyService2>();

        //    //var provider = services.BuildServiceProvider();
        //}

        /// <summary>
        /// 查找泛型依赖 & 没有指定实现类型也能查找到依赖
        /// </summary>
        [TestMethod]
        public void GetServiceWithGeneric()
        {
            var instance = GetServiceWithGeneric<IGenericService<Model1>>();

            Assert.IsNotNull(instance);
            Assert.AreEqual(instance.GetName(), "Service1");
        }

        private T GetServiceWithGeneric<T>()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IGenericService<Model1>>();
            services.AddSingleton<IGenericService<Model2>>();

            var provider = services.BuildServiceProvider();

            return provider.GetService<T>();
        }

        [TestMethod]
        public void GetServiceWithMultiGeneric()
        {
            var instance = GetServiceWithMultiGeneric<Model1, Model2>();

            Assert.IsNotNull(instance);
            Assert.AreEqual(instance.GetName(), "Multi Service2");
        }

        private IMultiGenericService<T1, T2> GetServiceWithMultiGeneric<T1, T2>()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IMultiGenericService<Model1, Model2>>();
            services.AddSingleton<IMultiGenericService<Model3, Model4>>();

            var provider = services.BuildServiceProvider();

            return provider.GetService<IMultiGenericService<T1, T2>>();
        }


        [TestMethod]
        public void 查询多接口类()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IMyServiceBase, MyServiceBase>();
            services.AddSingleton<IMyService3, MyService3>();
            services.AddSingleton<IMyService4, MyService4>();

            var provider = services.BuildServiceProvider();

            var service = (IMyServiceBase)provider.GetService(descriptor => {
                if (typeof(IMyServiceBase).IsAssignableFrom(descriptor.ServiceType))
                {
                    var _service = (IMyServiceBase)provider.GetService(descriptor.ImplementationType);
                    if ("MyService4".Equals(_service.key))
                    {
                        return true;
                    }
                }
                return false;
            });

            Assert.AreEqual(service.key, "MyService4");
        }
    }
}
