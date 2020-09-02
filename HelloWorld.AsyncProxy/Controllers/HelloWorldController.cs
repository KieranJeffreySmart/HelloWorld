using Microsoft.AspNetCore.Mvc;
namespace HelloWorld.AsyncProxy.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    [Route("[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private readonly ILogger<HelloWorldController> logger;
        private readonly IHelloWorldClientSettings settings;
        private readonly Func<HttpClient> clientFactory;

        public HelloWorldController(ILogger<HelloWorldController> logger, IHelloWorldClientSettings settings, Func<HttpClient> clientFactory)
        {
            this.logger = logger;
            this.settings = settings;
            this.clientFactory = clientFactory;
        }

        [HttpGet("Sync")]
        public string GetSync([FromQuery]string names)
        {
            logger.LogInformation(1000, $"GetSync(names:{names})");         
            var hellos = new List<string>();
            foreach(var name in names.Split(","))
            {
                hellos.Add(this.CallSync(name));
            }
            logger.LogInformation(1001, $"GetSync(names:{names})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet("SyncWithParallelForeach")]
        public string GetSyncWithParallelForeach([FromQuery]string names)
        {
            logger.LogInformation(1000, $"GetSyncWithParallelForeach(names:{names})");         
            var hellos = new ConcurrentBag<string>();
            Parallel.ForEach(names.Split(","), (name) =>
            {
                hellos.Add(this.CallSync(name));
            });
            logger.LogInformation(1001, $"GetSyncWithParallelForeach(names:{names})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet("SyncWithParallelInvoke")]
        public string GetSyncWithParallelInvoke([FromQuery]string namesA, [FromQuery]string namesB)
        {
            logger.LogInformation(1000, $"GetSyncWithParallelInvoke(namesA:{namesA}, namesB:{namesB})"); 

            var hellos = new ConcurrentBag<string>();

            Parallel.Invoke(() =>
            { 
                foreach(var name in namesA.Split(","))
                {
                    hellos.Add(this.CallSync(name));
                }
            },
            () => {
                foreach(var name in namesB.Split(","))
                {
                    hellos.Add(this.CallSync(name));
                }
            });

            logger.LogInformation(1001, $"GetSyncWithParallelInvoke(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet("SyncWithParallelInvokeAndForeach")]
        public string GetSyncWithParallelInvokeAndForeach([FromQuery]string namesA, [FromQuery]string namesB)
        {      
           logger.LogInformation(1000, $"SyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})"); 

            var hellos = new ConcurrentBag<string>();

            Parallel.Invoke(() =>
            { 
                Parallel.ForEach(namesA.Split(","), (name) =>
                {
                    hellos.Add(this.CallSync(name));
                });
            },
            () => 
            {                    
                Parallel.ForEach(namesB.Split(","), (name) =>
                {
                    hellos.Add(this.CallSync(name));
                });
            });

            logger.LogInformation(1001, $"SyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet("SyncToAsyncWithParallelInvokeAndForeach")]
        public string GetSyncToAsyncWithParallelInvokeAndForeach([FromQuery]string namesA, [FromQuery]string namesB)
        {      
            logger.LogInformation(1000, $"SyncToAsyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})"); 

            var hellos = new ConcurrentBag<string>();

            Parallel.Invoke(() =>
            { 
                Parallel.ForEach(namesA.Split(","), (name) =>
                {
                    hellos.Add(this.CallAsync(name).Result);
                });
            },
            () => 
            {                    
                Parallel.ForEach(namesB.Split(","), (name) =>
                {
                    hellos.Add(this.CallAsync(name).Result);
                });
            });

            logger.LogInformation(1001, $"SyncToAsyncWithParallelInvokeAndForeach(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet()]
        public async Task<string> GetAsync([FromQuery]string names)
        {
            logger.LogInformation(1000, $"GetAsync(names:{names})");
            var helloTasks = new List<Task<string>>();
            foreach(var name in names.Split(","))
            {
                helloTasks.Add(this.CallAsync(name));
            }

            var hellos = await Task.WhenAll(helloTasks);

            logger.LogInformation(1001, $"GetAsync(names:{names})");
            return String.Join("\r\n", hellos);
        }
                
        [HttpGet("AsyncWithTwoLists")]
        public async Task<string> GetAsyncWithTwoLists([FromQuery]string namesA, [FromQuery]string namesB)
        {
            logger.LogInformation(1001, $"GetAsyncWithTwoLists(namesA:{namesA}, namesB:{namesB})");
            var helloTasks = new List<Task<string>>();
            foreach(var name in namesA.Split(","))
            {
                helloTasks.Add(this.CallAsync(name));
            }

            foreach(var name in namesB.Split(","))
            {
                helloTasks.Add(this.CallAsync(name));
            }
            
            var hellos = await Task.WhenAll(helloTasks);

            logger.LogInformation(1001, $"GetAsyncWithTwoLists(namesA:{namesA}, namesB:{namesB})");
            return String.Join("\r\n", hellos);
        }

        [HttpGet("AsyncToSync")]
        public async Task<string> GetAsyncToSync([FromQuery]string names)
        {
            logger.LogInformation(1000, $"GetAsyncToSync(names:{names})");
            var hellos = new List<string>();
            foreach(var name in names.Split(","))
            {
                hellos.Add(this.CallSync(name));
            }

            logger.LogInformation(1001, $"GetAsyncToSync(names:{names})");
            return String.Join("\r\n", hellos);
        }

        private void CopyDoWorkHeaders(HttpClient client)
        {
            //var doWork = Request.Headers["X-dowork"].FirstOrDefault();
            //var min = Request.Headers["X-dowork-min"].FirstOrDefault();
            //var max = Request.Headers["X-dowork-max"].FirstOrDefault();

            //client.DefaultRequestHeaders.Add("X-dowork", doWork);
            //client.DefaultRequestHeaders.Add("X-dowork-min", min);
            //client.DefaultRequestHeaders.Add("X-dowork-max", max);
        }

        private async Task<string> CallAsync(string name)
        {
            var  client = clientFactory();
            this.CopyDoWorkHeaders(client);
            return await client.GetStringAsync(Path.Join(settings.Url, name));
        }

        private string CallSync(string name)
        {
            return CallAsync(name).Result;
        }
    }
}
