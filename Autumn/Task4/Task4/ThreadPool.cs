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
        private Queue<Action> tasks;
        private int threadNumber;
        private List<MyThread> threads;
        public ThreadPool(Queue<Action> list, int number)
        {
            threadNumber = number;
            tasks = list;
            threads = new List<MyThread>();
            for(int i = 0; i < threadNumber; i++)
            {
                MyThread thread = new MyThread(i.ToString());
                threads.Add(thread);
                thread.IsReady += StealTask;
            }
            while(tasks.Count != 0)
            {
                for (int i = 0; i < threadNumber; i++)
                {
                    threads[i].Enqueue(tasks.Dequeue());
                }
            }
            for (int i = 0; i < threadNumber; i++)
            {
                threads[i].Start();
            }
        }

        private void StealTask()
        {
            int max = threads[0].TaskCounter;
            int maxIndex = 0;
            for (int i = 0; i < threads.Count; i++)
            {
                if (threads[i].TaskCounter > max)
                {
                    max = threads[i].TaskCounter;
                    maxIndex = i;
                }
            }
            Enqueue(threads[maxIndex].Give());
        }

        public void Enqueue(Action task)
        {
            if(task == null)
            {
                return;
            }
            int min = threads[0].TaskCounter;
            int minIndex = 0;
            for(int i = 0; i < threads.Count; i++)
            {
                if(threads[i].TaskCounter < min)
                {
                    min = threads[i].TaskCounter;
                    minIndex = i;
                }
            }
            threads[minIndex].Enqueue(task);
        }

        public void Dispose()
        {
            for(int i = 0; i < threads.Count; i++)
            {
                threads[i].Dispose();
            }
        }
    }
}
