using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    public abstract class Worker
    {
        protected Thread myThread;
        protected List<int> data;
        protected bool isRunning;
        protected int sleep;

        protected Worker(AtomicLock locker, List<int> data, int sleep)
        {
            this.data = data;
            this.sleep = sleep;
            isRunning = true;
            myThread = new Thread(() => Run(locker));
            myThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            myThread.Join();
        }

        protected virtual void Run(AtomicLock locker)
        {

        }
    }
}
