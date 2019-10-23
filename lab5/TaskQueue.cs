using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lab5
{

    class TaskQueue
    {

        static object locker = new object();
        static Queue<TaskDelegate> QTask;
        private Thread[] threads;
        public Thread[] Threads
        {
            get { return threads; }
           
        }
        private volatile bool isStop = false;
        public bool IsStop
        {
            get { return isStop; }
            set { isStop = value; }
        }

        public TaskQueue(int threadCount)//создаем пул потоков
        {
            QTask = new Queue<TaskDelegate>();
            threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(Check);
                threads[i] = thread;
                thread.Start();
            }

        }
        public bool EnqueueTask(TaskDelegate task)
        {
            if (isStop)
                return false;
            QTask.Enqueue(task);
            return true;
        }


        public void Check()//проверяем, есть ли в очереди задачи
        {
            while (!isStop || (QTask.Count > 0))
            {//если еще не было конца или очередь не пуста
                TaskDelegate task = null;
                lock (locker)//блокировка, на время обращения к очереди
                {
                    if (QTask.Count > 0)
                        task = QTask.Dequeue();
                }
                task?.Invoke();//вызов задачи 
            }

        }
    }
}
