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
        List<Action> actList = new List<Action>();
        Semaphore sem = new Semaphore(1, 1);
        public int ended = 0;

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
                threadList.Add(new Thread(WorkInThread));
            }
        }

        public void Enqueue(Action a) // добавление задачи в поток
        {
            sem.WaitOne();
            actList.Add(a);
            sem.Release();
        }

        public void Dispose() // освободить ресурсы
        {
            flag = false;
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
                sem.WaitOne();
                if (actList.Count() == 0)
                {
                    sem.Release();
                    continue;
                }
                else
                {
                    myAct = actList[0];
                    actList.RemoveAt(0);
                    ended++;
                    sem.Release();
                }
                myAct();
            }
        }
    }
}
