using Microsoft.AspNetCore.Builder;

namespace HelloWorld
{
    public static class DoWorkMiddlewareExtensions
    {
        public static IApplicationBuilder UseDoWork(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DoWorkMiddleware>();
        }
    }
}
