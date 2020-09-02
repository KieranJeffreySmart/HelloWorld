using HelloWorld.AsyncProxy.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HelloWorld.AsyncProxy.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetAsyncs()
        {
            var logger = NSubstitute.Substitute.For<ILogger<HelloWorldController>>();
            var setting = new HelloWorldClientSettings("http://localhost:5000/HelloWorld");
            Func<HttpClient> clientFactory = () => { return new HttpClient(); };
            var controller = new HelloWorldController(logger, setting, clientFactory);

            var result = await controller.GetAsync("Bob");

            Assert.Equals("Hello Bob", result);
        }
    }
}
