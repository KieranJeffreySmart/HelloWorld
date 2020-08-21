namespace HelloWorld.Tasks
{
    using System.Threading;
    using Steeltoe.Common.Tasks;

    public class DelayTaskDecorator<TApplicationTask> : IApplicationTask 
        where TApplicationTask : class, IApplicationTask, new()
    {
        private readonly TApplicationTask innerTask = new TApplicationTask();
        public string Name => innerTask.Name;

        private readonly int delay;

        public DelayTaskDecorator(ITaskDelayConfiguration configuration)
        {
            delay = configuration.ForceDelay;
        }

        public void Run()
        {
            Thread.Sleep(delay);
            innerTask.Run();
        }
    }
}