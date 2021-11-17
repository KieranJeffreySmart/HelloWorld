using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Steeltoe.Management.CloudFoundry;

namespace HelloWorld.AsyncProxy
{
    public class Startup
    {
        private HttpClient SINGLETON_CLIENT = new HttpClient();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddCloudFoundryActuators(Configuration);

            services.AddControllers();

            services.AddSingleton<IHelloWorldClientSettings>(new HelloWorldClientSettings(Configuration["HELLOWORLD_URL"]));
            services.AddSingleton<Func<HttpClient>>(() => Configuration.GetValue<bool>("PROXY_SINGLETONHTTPCLIENT") ? SINGLETON_CLIENT : new HttpClient());

            var diagClient = new DiagnosticsClient(System.Diagnostics.Process.GetCurrentProcess().Id);
            //var providers = new List<EventPipeProvider>()
            //{
            //    new EventPipeProvider("Microsoft-Windows-DotNETRuntime",
            //        EventLevel.Informational, (long)ClrTraceEventParser.Keywords.GC)
            //};

            //using (EventPipeSession session = diagClient.StartEventPipeSession(providers, false))
            //{
            //    var source = new EventPipeEventSource(session.EventStream);

            //    try
            //    {
            //        source.Process();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error encountered while processing events");
            //        Console.WriteLine(e.ToString());
            //    }
            //}
            services.AddSingleton(diagClient);

            services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(Logger<>)));
        }
 
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCloudFoundryActuators();
       
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
