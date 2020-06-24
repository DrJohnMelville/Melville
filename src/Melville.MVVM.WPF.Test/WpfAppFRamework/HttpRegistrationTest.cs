﻿using System.Net;
using System.Net.Http;
using Melville.IOC.IocContainers;
using Melville.MVVM.CSharpHacks;
using Melville.WpfAppFramework.HttpsServices;
using Melville.WpfAppFramework.StartupBases;
using Xunit;

namespace Melville.MVVM.WPF.Test.WpfAppFRamework
{
    public class HttpRegistrationTest
    {
        [Fact]
        public void HttpClients()
        {
            var ioc = new IocContainer();
            ioc.RegisterHttpClientWithSingleSource();

            var c1 = ioc.Get<HttpClient>();
            Assert.False((bool)c1.GetField("_disposeHandler"));
        }

    }
}