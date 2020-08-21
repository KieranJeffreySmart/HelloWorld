namespace ManagedTasks
{
    using System.Collections.Generic;
    using Steeltoe.Common.Tasks;

    public abstract class AggregateApplicationTask : IApplicationTask
    {
        public abstract string Name { get; }

        protected abstract IEnumerable<IApplicationTask> Tasks { get; }

        public void Run()
        {
            foreach (var task in Tasks)
            {
                task.Run();
            }
        }
    }
}