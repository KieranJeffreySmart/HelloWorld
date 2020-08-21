namespace HelloWorld.Batch.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Steeltoe.Common.Tasks;

    public class App
    {
        private readonly List<IApplicationTask> tasks;

        public App(IEnumerable<IApplicationTask> tasks)
        {
            this.tasks = tasks.ToList();
        }

        public void Run()
        {
            while(true)
            {
                //Run indefinitely
            }
        }

        private void WriteCommandList()
        {
            Console.WriteLine("Execute A Task:");
            for (var i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i}: {tasks[i].Name}");
            }

            Console.WriteLine($"q: quit");

            Console.Write($"Command [0-{tasks.Count}, q]: ");
        }
    }
}