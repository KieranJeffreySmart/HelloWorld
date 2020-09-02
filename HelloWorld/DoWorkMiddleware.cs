using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HelloWorld
{
    public class DoWorkMiddleware
    {
        private readonly RequestDelegate _next;
        Random random = new Random();
        const int MAX_DOWORK = 20;

        public DoWorkMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (int.TryParse(context.Request.Headers["X-dowork"].FirstOrDefault(), out var doWorkInSeconds))
            {
                if (doWorkInSeconds < 0)
                {
                    int.TryParse(context.Request.Headers["X-dowork-min"].FirstOrDefault(), out var min);
                    int.TryParse(context.Request.Headers["X-dowork-max"].FirstOrDefault(), out var max);
                    max = max == 0 ? MAX_DOWORK : max;

                    doWorkInSeconds = random.Next(min, max);
                }

                var doWork = Math.Min(doWorkInSeconds, MAX_DOWORK) * 1000;
                
                await Task.Delay(doWork);
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
        
    }
}