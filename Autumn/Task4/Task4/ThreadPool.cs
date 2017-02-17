using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task4
{
    public class ThreadPool : IDisposable
    {
        private int threadNumber;
        private List<MyThread> threads;
        private Random rand;
        public ThreadPool(int number)
        {
            threadNumber = number;
            threads = new List<MyThread>();
            for(int i = 0; i < threadNumber; i++)
            {
                MyThread thread = new MyThread(i.ToString());
                thread.IsReady += StealTask;
                threads.Add(thread);
            }
            rand = new Random();
        }

        private void StealTask()
        {
            int number = rand.Next(threadNumber);
            Enqueue(threads[number].Give());
        }

        public void Enqueue(Action task)
        {
            if(task == null)
            {
                return;
            }
            int number = rand.Next(threadNumber);
            threads[number].Enqueue(task);
        }

        public void Dispose()
        {
            for(int i = 0; i < threads.Count; i++)
            {
                threads[i].Dispose();
            }
            threads = new List<MyThread>();
        }
    }
}
