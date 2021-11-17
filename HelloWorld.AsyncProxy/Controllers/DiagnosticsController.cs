using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.NETCore.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelloWorld.AsyncProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private readonly string DEFAULT_APPDUMP_FOLDER = "appDumps";
        string getAppDumpPath(Guid id) => Path.Combine("./", DEFAULT_APPDUMP_FOLDER, $"app-dump-{id}.dmp");

        DiagnosticsClient client;

        public DiagnosticsController(DiagnosticsClient client)
        {
            this.client = client;
        }



        [HttpGet("Processes")]
        public IEnumerable<object> GetApps()
        {
            return DiagnosticsClient.GetPublishedProcesses().Select(id => System.Diagnostics.Process.GetProcessById(id)).Select(p => new
            {
                p.Id,
                p.ProcessName,
                p.StartTime,
                p.HasExited,
                ExitTime = !p.HasExited ? DateTime.MinValue : p.ExitTime,
                ExitCode = !p.HasExited ? 0 : p.ExitCode,
                p.MachineName,
                ThreadCount = p.Threads.Count
            }).ToList();
        }
    }
}
