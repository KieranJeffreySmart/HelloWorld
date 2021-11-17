using Microsoft.AspNetCore.Mvc;
namespace HelloWorld.Controllers
{
    using System.Threading.Tasks;

    [Route("[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {   
            return await Task.FromResult("Hello World");
        }

        // GET api/values/5
        [HttpGet("{name}")]
        public async Task<ActionResult<string>> Get(string name)
        {
            return await Task.FromResult($"Hello {name}");
        }
    }
}
