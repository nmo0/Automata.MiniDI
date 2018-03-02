﻿using System;
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


    }
}