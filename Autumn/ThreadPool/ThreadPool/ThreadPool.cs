using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace ThreadPool
{
    public class ThreadPool: IDisposable
    {
        private bool _finish = false;
        private const int NumThread = 3;
        private Queue<Action> taskQueue = new Queue<Action>();
        private object threadLock = new object();
        private Thread[] threadArr = new Thread[3];
        private int numPortion = 0;
        private bool portion = false;
        public struct newPortion
        {
            public bool portion;
            public int numPortion;
            
            public newPortion(bool portion, int numPortion)
            {
                this.portion = portion;
                this.numPortion = numPortion;
            }
        }
        newPortion threadPoolPortion = new newPortion(false, 0);
        public void Enqueue(Action task)
        {
            lock (threadLock)
            {
                taskQueue.Enqueue(task);
                threadPoolPortion.portion = true;
                threadPoolPortion.numPortion += 1;
            }
        }

        public void Dispose()
        {
            taskQueue.Clear();
            _finish = true;
            Console.WriteLine("Dispose finished");
        }

        public void Start()
        {
            int j=0;
            while(!_finish)
            {
                if (taskQueue.Count != 0)
                {
                    threadArr[j % 3] = new Thread(() =>
                    {
                        numPortion = threadPoolPortion.numPortion;
                        portion = threadPoolPortion.portion;
                        Action act;
                        lock (threadLock)
                        {
                            act = taskQueue.Dequeue();
                        }
                        act();
                    });

                    if (_finish)
                        break;

                    threadArr[j % 3].Start();
                    j++;
                }
                else
                {
                    lock (threadLock)
                    {
                        if (numPortion == threadPoolPortion.numPortion)
                            portion = false;
                    }

                    if (threadArr[1] == Thread.CurrentThread)
                        while (!portion)
                            Thread.Sleep(1);
                    else
                        break;

                    if (_finish)
                        break;
                }
            }
        }




    }
}
