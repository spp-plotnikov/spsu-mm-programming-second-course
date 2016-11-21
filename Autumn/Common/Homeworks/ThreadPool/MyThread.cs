using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    public class MyThread
    {
        Thread mySubThread_;
        Queue<Action> tasks_;
        bool work_ = true;
        ManualResetEvent finished_ = new ManualResetEvent(false);
        ManualResetEvent working_ = new ManualResetEvent(false);

        public MyThread(Queue<Action> acts, int num)
        {
            tasks_ = acts;
            mySubThread_ = new Thread(MyThreadStart);
            mySubThread_.Start(num);
        }

        // check whether thread is busy or not
        public bool IsWorking
        {
            get
            {
                return working_.WaitOne(0);
            }
        }

        // notify that some new work appeared
        public void Notify()
        {
            working_.Set();
        }

        public void MyThreadStart(object num)
        {
            while (work_)
            {
                // prevent from other threads changes of the tasks queue
                Monitor.Enter(tasks_);

                // if there is something to do
                if (tasks_.Count > 0)
                {
                    Action myCurAction = tasks_.Dequeue();
                    myCurAction();
                    Console.WriteLine("The task is completed by thread number {0}", num);
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------------");
                    Monitor.Exit(tasks_);
                }
                else
                {
                    // don't need to change the tasks queue, finish blocking immediately
                    Monitor.Exit(tasks_);

                    // waiting for some new job or to finish the thread
                    working_.Reset();
                    finished_.Set();
                    working_.WaitOne();
                    finished_.Reset();
                }
            }
        }

        // end the thread
        public void Finish()
        {
            work_ = false;
            finished_.WaitOne();
            Notify();
            mySubThread_.Join();
        }
    }
}
