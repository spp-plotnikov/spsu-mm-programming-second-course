using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Producer_consumer
{
    class Buffer
    {
        private List<int> buf;
        private Mutex mutex;

        public Buffer()
        {
            buf = new List<int>();
            mutex = new Mutex();
        }

        public void BufEnque(int x)
        {
            mutex.WaitOne();
            buf.Add(x);
            mutex.ReleaseMutex();
        }

        public int GetBufSize()
        {
            return buf.Count();
        }

        public int BufDeque()
        {
            int x;
            mutex.WaitOne();
            if (buf.Count() == 0)
            {
                x = default(int);
            }
            else
            {
                x = buf.First();
                buf.Remove(x);
            }
            mutex.ReleaseMutex();
            return x;
        }

    }
}
