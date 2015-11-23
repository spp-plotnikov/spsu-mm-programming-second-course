using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadPool
{
    class MyThread
    {
        bool IsEnable; 
        public Queue<Action> QueueOfMethods;
        Thread MainThread;
        public Mutex LockerOfQueue; 
        int index;

        void GettingAction()
        {
            while (IsEnable)
            {
                while (!IsEmpty())
                {
                    LockerOfQueue.WaitOne();
                    Action tmp = QueueOfMethods.Dequeue();
                    LockerOfQueue.ReleaseMutex();
                    tmp();
                }
            }
        }

        public MyThread (int n)
        {
            IsEnable = true;
            QueueOfMethods = new Queue<Action>();
            MainThread = new Thread(GettingAction);
            MainThread.Start();
            LockerOfQueue = new Mutex();
            index = n;
        }
        
        public void Enqueue(Action a)
        {
            LockerOfQueue.WaitOne();
            QueueOfMethods.Enqueue(a);
            LockerOfQueue.ReleaseMutex();
        }
        
        public bool IsEmpty()
        {
            LockerOfQueue.WaitOne();
            if (QueueOfMethods.Count == 0)
            {
                LockerOfQueue.ReleaseMutex();
                return true;
            }
            else
            {
                LockerOfQueue.ReleaseMutex();
                return false;
            }
        }

        public void EndWork()
        {
            IsEnable = false;
        }
    }
}
