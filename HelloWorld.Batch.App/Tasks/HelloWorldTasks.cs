namespace ManagedTasks
{
    using System;
    using System.Collections.Generic;
    using Steeltoe.Common.Tasks;

    public class HelloWorldTask: IApplicationTask
    {
        public void Run()
        {
            Console.WriteLine("Hello World!");
        }

        public string Name => "SayHello";
    }

    public class GoodByeWorldTask : IApplicationTask
    {
        public void Run()
        {
            Console.WriteLine("Good Bye World!");
        }

        public string Name => "SayGoodBye";
    }

    public class MerryXmasWorldTask : IApplicationTask
    {
        public void Run()
        {
            Console.WriteLine("Merry Xmas World!");
        }

        public string Name => "SayMerryXmas";
    }

    public class HappyNewYearTask : IApplicationTask
    {
        public void Run()
        {
            Console.WriteLine("Happy New Year World!");
        }

        public string Name => "SayHappyNewYear";
    }

    public class ForceExceptionTask : IApplicationTask
    {
        public void Run()
        {
            throw new Exception("Forced Exception");
        }

        public string Name => "ForceException";
    }

    public class AllGreetingsAggregateApplicationTask : AggregateApplicationTask
    {
        public override string Name => "AllGreetings";

        protected override IEnumerable<IApplicationTask> Tasks { get; } = new List<IApplicationTask>
        {
            new HelloWorldTask(),
            new MerryXmasWorldTask(),
            new HappyNewYearTask(),
            new GoodByeWorldTask()
        };
    }
}