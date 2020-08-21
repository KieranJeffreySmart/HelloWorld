namespace HelloWorld.Tasks
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Steeltoe.Common.Tasks;

    public class ApplicationTaskFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ApplicationTaskFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IApplicationTask Create(string taskName)
        {
            var scope = serviceProvider.CreateScope().ServiceProvider;
            if (taskName == null) return null;

            var task = scope.GetServices<IApplicationTask>().FirstOrDefault(x => x.Name.ToLower() == taskName.ToLower());
            if (task != null)
            {
                return task;
            }

            var logger = scope.GetService<ILoggerFactory>()
                .CreateLogger("CloudFoundryTasks");
            logger.LogError($"No task with name {taskName} is found registered in service container");

            return null;
        }
    }
}