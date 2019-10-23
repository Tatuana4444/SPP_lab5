using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace lab5
{
    delegate void TaskDelegate();
    class Program
    {
        public static void WriteHello()
        {
            Console.WriteLine("Hello world!");
        }
        static void Main(string[] args)
        {
            TaskDelegate[] task = new TaskDelegate[10];
            for (int i = 0; i < 10; i++)
                task[i] = WriteHello;
            Parallel.WaitAll(task);
            Console.WriteLine("ALL");
            Console.ReadKey();
        }
        class Parallel
        {
            public static void WaitAll(TaskDelegate[] task)
            {
                TaskQueue taskQueue = new TaskQueue(task.Length);
                Queue<TaskDelegate> QTask = new Queue<TaskDelegate>(task);
                for (int i = 0; i < task.Length; i++)
                    taskQueue.EnqueueTask(task[i]);
                taskQueue.IsStop = true;
                for (int i = 0; i < task.Length; i++)
                    taskQueue.Threads[i].Join();
            }

        }
    }
}
