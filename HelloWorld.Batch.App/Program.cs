namespace HelloWorld.Batch.App
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using ManagedTasks;
    using Steeltoe.Management.TaskCore;
    using Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, args);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var taskName = config.GetValue<string>("runtask");

            var task = serviceProvider.GetRequiredService<ApplicationTaskFactory>().Create(taskName);
            if (task == null)
            {
                Console.WriteLine($"Task {taskName} not found");
            }

            task.Run();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, string[] args)
        {
            serviceCollection.AddTransient<ApplicationTaskFactory>();

            serviceCollection.AddTask<DelayTaskDecorator<HelloWorldTask>>();
            serviceCollection.AddTask<DelayTaskDecorator<MerryXmasWorldTask>>();
            serviceCollection.AddTask<DelayTaskDecorator<HappyNewYearTask>>();
            serviceCollection.AddTask<DelayTaskDecorator<GoodByeWorldTask>>();
            serviceCollection.AddTask<DelayTaskDecorator<ForceExceptionTask>>();

            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

            var timeDelayConfig = new TaskDelayConfiguration { ForceDelay = 1000 };
            serviceCollection.AddSingleton<ITaskDelayConfiguration>(timeDelayConfig);
            serviceCollection.AddSingleton<IConfiguration>(configuration);
        }
    }
}
