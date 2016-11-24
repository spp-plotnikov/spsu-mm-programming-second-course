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
        int numOfTasks;
        public int numForPool;
        List<MyTask> taskList = new List<MyTask>();
        Pool myPool;
        
        public PoolManager(int p, int t)
        {
            numForPool = p;
            numOfTasks = t;
        }

        void CreateTasks()
        {
            for (int i = 0; i < numOfTasks; i++)
            {
                taskList.Add(new MyTask(i));
            }
        }
        
        public void StartWork()
        {
            myPool = new Pool(numForPool);

            CreateTasks();

            for (int i = 0; i < numOfTasks; i++)
            { 
                myPool.Enqueue(taskList[0].StartWork);
                taskList.RemoveAt(0);
            }

            while(numOfTasks != myPool.ended) { }
            myPool.Dispose();

        }
        
    }
}

