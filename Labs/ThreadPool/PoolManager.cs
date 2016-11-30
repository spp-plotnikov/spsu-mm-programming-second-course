using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    class PoolManager 
    {
        public int numForPool;
        List<MyTask> taskList = new List<MyTask>();
        Pool myPool;
        public PoolManager(int p)
        {
            numForPool = p;
        }
                       
        public void StartWork()
        {
            myPool = new Pool(numForPool);
            int i = 0;
            while (!Console.KeyAvailable)
            {
                taskList.Add(new MyTask(i));
                myPool.Enqueue(taskList.Last().StartWork);
                i++;
                if (i == int.MaxValue) i = 0;
            }
        }
        
        public void StopWork()
        {
            myPool.Dispose();
        }
    }
}

