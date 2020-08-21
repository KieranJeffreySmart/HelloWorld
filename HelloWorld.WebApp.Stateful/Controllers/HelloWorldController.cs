using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace HelloWorld.WebApp.Stateful.Controllers
{
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private const string HISTORY_LOG_FOLDER = "./log";
        private const string HISTORY_LOG_PATH = HISTORY_LOG_FOLDER + "/history.txt";
        private ILogger<HelloWorldController> logger;

        public HelloWorldController(ILogger<HelloWorldController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<string>> Get(string name)
        {
            var response = $"Hello {name}";

            if (await AlreadySaidHello(name))
            {
                response = $"Welcome back {name}";
                logger.LogInformation("Welcomed returning user");
            }
            else
            {
                await WriteHistoryLogFile(response);
                logger.LogInformation("Welcomed new user");
            }

            return response;
        }

        private async Task WriteHistoryLogFile(string line)
        {
            if (!System.IO.Directory.Exists(HISTORY_LOG_FOLDER))
            {
                logger.LogInformation("Creating new directory for user history");
                System.IO.Directory.CreateDirectory(HISTORY_LOG_FOLDER);
            }

            await System.IO.File.AppendAllTextAsync(HISTORY_LOG_PATH, $"{DateTime.Now}: {line}\r\n");
        }

        private async Task<bool> AlreadySaidHello(string name)
        {
            if (!System.IO.File.Exists(HISTORY_LOG_PATH))
            {
                return false;
            }

            var lines = await System.IO.File.ReadAllLinesAsync(HISTORY_LOG_PATH);
            return lines.Any(l => l.ToLower().Contains(name.ToLower()));
        }
    }
}
