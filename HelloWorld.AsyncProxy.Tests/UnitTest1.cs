using HelloWorld.AsyncProxy.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace HelloWorld.AsyncProxy.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var logger = NSubstitute.Substitute.For<ILogger<HelloWorldController>>();
            var setting = new HelloWorldClientSettings("http://localhost:5000/HelloWorld");
            Func<HttpClient> clientFactory = () => { return new HttpClient(); };
            var controller = new HelloWorldController(logger, setting, clientFactory);

            var result = controller.GetAsync("Bob").Result;

            Assert.AreEqual("Hello Bob", result);
        }
    }
}
