using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    public class ThreadPool : IDisposable
    {
        private int _numOfThreads = 5;
        private MyThread[] _threads = new MyThread[5];
        private Queue<Action> _poolActions = new Queue<Action>();

        public ThreadPool()
        {
            for (int i = 0; i < _numOfThreads; i++)
            {
                _threads[i] = new MyThread(_poolActions);
            }
        }

        public void AddToPool(Action newAction)
        {
            Monitor.Enter(_poolActions);
            _poolActions.Enqueue(newAction);
            int itr = 0;

            // find the thread to do the job
            while (itr < _numOfThreads && _threads[itr].Busy != false)
            {
                itr++;
            }

            if(itr < _numOfThreads)
            {
                _threads[itr].GiveSignal();
            }

            Monitor.Exit(_poolActions);
        }

        public void Dispose()
        {
            for (int i = 0; i < _numOfThreads; i++)
            {
                _threads[i].Finish();
            }

            Console.WriteLine("All threads deleted");
        }
       
    }
}
