using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Semaphores
{
    public class Consumer
    {
        private int _id;

        public Consumer(int Id)
        {
            _id = Id;

        }

        public void Run(ref bool flag, ref Semaphore Lock, ref List<int> _buf)
        {
            while (flag)
            {
                Lock.WaitOne();
                if (_buf.Count() > 0)
                {
                    int item = _buf.First();
                    if (_buf.Remove(item))
                        Console.WriteLine("Consumer №{0} get  {1}", _id, item);
                }
                Lock.Release();
                Thread.Sleep(1000);
            }
        }
    }

    public class Producer
    {
        private int _id;

        public Producer(int Id)
        {
            _id = Id;

        }

        public void Run(ref bool flag, ref Semaphore Lock, ref List<int> buf)
        {
            while (flag)
            {
                Lock.WaitOne();
                buf.Add(_id);
                Console.WriteLine("Producer №{0} add {1}", _id, _id);
                Lock.Release();
                Thread.Sleep(1000);
            }
        }
    }

}
