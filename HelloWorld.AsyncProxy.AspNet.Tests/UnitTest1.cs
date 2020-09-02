using System;
using System.Net.Http;
using System.Threading.Tasks;
using HelloWorld.AsyncProxy.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HelloWorld.AsyncProxy.AspNet.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetAsync()
        {
            var controller = new HelloWorldController();

            var result = await controller.GetAsync("Bob,Jack,Sally,Anne,Jane,John");
        }

        [TestMethod]
        public void GetSync()
        {
            var controller = new HelloWorldController();

            var result = controller.GetSync("Bob,Jack,Sally,Anne,Jane,John");
        }

        [TestMethod]
        public void GetSyncToAsyncWithParallelInvokeAndForeach()
        {
            var controller = new HelloWorldController();

            var result = controller.GetSyncToAsyncWithParallelInvokeAndForeach("Bob,Jack,Sally,Anne,Jane,John", "Bob,Jack,Sally,Anne,Jane,John");
        }

        [TestMethod]
        public async Task GetAsyncWithTwoLists()
        {
            var controller = new HelloWorldController();

            var result = await controller.GetAsyncWithTwoLists("Bob,Jack,Sally,Anne,Jane,John", "Bob,Jack,Sally,Anne,Jane,John");
        }
    }
}
