using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Pool : IDisposable
    {
        int numOfThr;
        List<Thread> threadList = new List<Thread>();
        bool flag = true;
        List<Action> taskList = new List<Action>();

        public Pool(int num)
        {
            numOfThr = num;
            CreatePool();

            foreach (var t in threadList)
            {
                t.Start();
            }
        }

        void CreatePool()
        {
            for (int i = 0; i < numOfThr; i++)
            {
                Thread t = new Thread(WorkInThread);
                t.Name = "Thread_" + i;
                threadList.Add(t);
            }            
        }

        public void Enqueue(Action a) // добавление задачи в поток
        {
            lock (taskList)
            {
                taskList.Add(a);
                Monitor.Pulse(taskList);
            }
        }

        public void Dispose() // освободить ресурсы
        {
            flag = false;
            lock(taskList)
            {
                Monitor.PulseAll(taskList);
            }
            foreach (var t in threadList)
            {
                t.Join();
            }
        }

        void WorkInThread()
        {
            Action myAct = () => { };
            while(flag)
            {
                lock (taskList)
                {
                    if (taskList.Count() == 0)
                    {
                        Monitor.Wait(taskList);
                    }
                    if (!flag)
                    {
                        break;
                    }
                    myAct = taskList[0];
                    taskList.RemoveAt(0);
                }
                myAct();                
            }
        }
    }
}
