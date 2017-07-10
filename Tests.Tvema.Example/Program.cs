namespace Tests.Tvema.Example
{
    using System;
    using System.Threading;
    using static System.Linq.Enumerable;

    public class Program
    {
        private const int TaskCount = 20;


        static void Main(string[] args)
        {
            var pool = new FixedThreadPool(5);

            foreach (var t in PrepareTasks(TaskCount))
            {
                pool.Execute(t.Task, t.Priority);
            }

            Console.WriteLine("Tasks are queued, press <ENTER> to stop the pool.");
            Console.ReadLine();

            pool.Stop();

            Console.WriteLine("All tasks completed. Press <ENTER> to exit.");
            Console.ReadLine();
        }


        private static TaskWithPriority[] PrepareTasks(int count)
        {
            var random = new Random();

            return Range(0, count)
                .Select(_ => random.Next(0, 3))
                .ToArray()
                .Select(p => new TaskWithPriority(new TestTask(), (Priority) p))
                .ToArray();
        }


        private class TestTask : ITask
        {
            private readonly Random _random = new Random();

            public void Execute() => Thread.Sleep(_random.Next(2, 5) * 1000);
        }

        private struct TaskWithPriority
        {
            public TaskWithPriority(ITask task, Priority priority)
            {
                Task = task;
                Priority = priority;
            }


            public ITask Task { get; }

            public Priority Priority { get; }
        }
    }
}
