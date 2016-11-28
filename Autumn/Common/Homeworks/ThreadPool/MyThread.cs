using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    class MyThread
    {
        private Thread _mySubThread;
        private bool _work = true;
        private ManualResetEvent _working = new ManualResetEvent(false);
        private Queue<Action> _poolActions = new Queue<Action>();

        public MyThread(Queue<Action> poolActions)
        {
            _poolActions = poolActions;
            _mySubThread = new Thread(MyThreadStart);
            _mySubThread.Start();
        }

        public bool Busy
        {
            get
            {
                return _working.WaitOne(0);
            }
        }

        public void GiveSignal()
        {
            _working.Set();
        }

        public void MyThreadStart()
        {
            while (_work)
            {
                // prevent from other threads changes of the tasks queue
                Monitor.Enter(_poolActions);

                // if there is something to do
                if (_poolActions.Count > 0)
                {
                    Action myCurAction = _poolActions.Dequeue();
                    Monitor.Exit(_poolActions);
                    myCurAction();
                }
                else
                {
                    // don't need to change the tasks queue, finish blocking immediately
                    Monitor.Exit(_poolActions);

                    // waiting for some new job or to finish the thread
                    _working.Reset();
                    _working.WaitOne();
                }
            }
        }

        // end the thread
        public void Finish()
        {
            _work = false;
            _working.Set();
            _mySubThread.Join();
        }
    }
}
