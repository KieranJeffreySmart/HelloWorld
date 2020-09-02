using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HelloWorld.AsyncProxy.Framework
{
    [Route("HelloWorld")]
    public class HelloWorldController : ApiController
    {

        private readonly ILogger<HelloWorldController> logger;
        private readonly IHelloWorldClientSettings settings;
        private readonly Func<HttpClient> clientFactory;

        public HelloWorldController()
        {
            logger = new LoggerFactory().CreateLogger<HelloWorldController>();
            this.settings = new HelloWorldClientSettings("http://localhost:5000/HelloWorld");
            this.clientFactory = () => { return new HttpClient(); };
        }

        [HttpGet()]
        [Route("Sync")]
        public string GetSync([FromUri] string names)
        {
            logger.LogInformation(1000, $"GetSync(names:{names})");
            var hellos = new List<string>();
            foreach (var name in names.Split(','))
            {
                hellos.Add(this.CallSync(name));
            }
            logger.LogInformation(1001, $"GetSync(names:{names})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        [Route("SyncWithParallelForeach")]
        public string GetSyncWithParallelForeach([FromUri] string names)
        {
            logger.LogInformation(1000, $"GetSyncWithParallelForeach(names:{names})");
            var hellos = new ConcurrentBag<string>();
            Parallel.ForEach(names.Split(','), (name) =>
            {
                hellos.Add(this.CallSync(name));
            });
            logger.LogInformation(1001, $"GetSyncWithParallelForeach(names:{names})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        [Route("SyncWithParallelInvoke")]
        public string GetSyncWithParallelInvoke([FromUri] string namesA, [FromUri] string namesB)
        {
            logger.LogInformation(1000, $"GetSyncWithParallelInvoke(namesA:{namesA}, namesB:{namesB})");

            var hellos = new ConcurrentBag<string>();

            Parallel.Invoke(() =>
            {
                foreach (var name in namesA.Split(','))
                {
                    hellos.Add(this.CallSync(name));
                }
            },
            () => {
                foreach (var name in namesB.Split(','))
                {
                    hellos.Add(this.CallSync(name));
                }
            });

            logger.LogInformation(1001, $"GetSyncWithParallelInvoke(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        [Route("SyncWithParallelInvokeAndForeach")]
        public string GetSyncWithParallelInvokeAndForeach([FromUri] string namesA, [FromUri] string namesB)
        {
            logger.LogInformation(1000, $"SyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})");

            var hellos = new ConcurrentBag<string>();

            Parallel.Invoke(() =>
            {
                Parallel.ForEach(namesA.Split(','), (name) =>
                {
                    hellos.Add(this.CallSync(name));
                });
            },
            () =>
            {
                Parallel.ForEach(namesB.Split(','), (name) =>
                {
                    hellos.Add(this.CallSync(name));
                });
            });

            logger.LogInformation(1001, $"SyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        [Route("SyncToAsyncWithParallelInvokeAndForeach")]
        public string GetSyncToAsyncWithParallelInvokeAndForeach([FromUri] string namesA, [FromUri] string namesB)
        {
            logger.LogInformation(1000, $"SyncToAsyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})");

            var hellos = new ConcurrentBag<string>();

            Parallel.Invoke(() =>
            {
                Parallel.ForEach(namesA.Split(','), (name) =>
                {
                    hellos.Add(this.CallAsync(name).Result);
                });
            },
            () =>
            {
                Parallel.ForEach(namesB.Split(','), (name) =>
                {
                    hellos.Add(this.CallAsync(name).Result);
                });
            });

            logger.LogInformation(1001, $"SyncToAsyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        public async Task<string> GetAsync([FromUri] string names)
        {
            logger.LogInformation(1000, $"GetAsync(names:{names})");
            var helloTasks = new List<Task<string>>();
            foreach (var name in names.Split(','))
            {
                helloTasks.Add(this.CallAsync(name));
            }

            var hellos = await Task.WhenAll(helloTasks);

            logger.LogInformation(1001, $"GetAsync(names:{names})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        [Route("AsyncWithTwoLists")]
        public async Task<string> GetAsyncWithTwoLists([FromUri] string namesA, [FromUri] string namesB)
        {
            logger.LogInformation(1001, $"GetAsyncWithTwoLists(namesA:{namesA}, namesB:{namesB})");
            var helloTasks = new List<Task<string>>();
            foreach (var name in namesA.Split(','))
            {
                helloTasks.Add(this.CallAsync(name));
            }

            foreach (var name in namesB.Split(','))
            {
                helloTasks.Add(this.CallAsync(name));
            }

            var hellos = await Task.WhenAll(helloTasks);

            logger.LogInformation(1001, $"GetAsyncWithTwoLists(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        [Route("AsyncToSync")]
        public async Task<string> GetAsyncToSync([FromUri] string names)
        {
            logger.LogInformation(1000, $"GetAsyncToSync(names:{names})");
            var hellos = new List<string>();
            foreach (var name in names.Split(','))
            {
                hellos.Add(this.CallSync(name));
            }

            logger.LogInformation(1001, $"GetAsyncToSync(names:{names})");
            return String.Join("\r\n", hellos);
        }

        private void AddDoWorkHeaders(HttpClient client)
        {
            var doWork = 10;
            client.DefaultRequestHeaders.Add("X-dowork", doWork.ToString());
        }

        private async Task<string> CallAsync(string name)
        {
            var client = clientFactory();
            this.AddDoWorkHeaders(client);
            return await client.GetStringAsync(Path.Combine(settings.Url, name));
        }

        private string CallSync(string name)
        {
            return CallAsync(name).Result;
        }
    }
}