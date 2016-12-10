using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Semaphores
{
    public static class SemManager
    {
        private static int _pause = 1000;
        private static bool _flag = true;

        static List<int> _buf = new List<int>();

        private static int _qProd = 1;
        private static int _qCons = 3;

        public static List<Thread> PList = new List<Thread>(); // storage
        public static List<Thread> CList = new List<Thread>();

        public static Semaphore Lock = new Semaphore(1, 1);

        static public  void Emulation (int numProd, int numCons)
        {
            _qCons = numCons;
            _qProd = numProd;

            Console.WriteLine("Press 'Enter' to stop...");
            //Thread.Sleep(wait * 2);
            for (int i = 0; i < _qProd; i++)
            {
                Producer tmp = new Producer(i);
                Thread prod = new Thread(() => tmp.Run());
                PList.Add(prod);
                prod.Start();
            }

            for (int i = 0; i < _qCons; i++)
            {
                Consumer consumerObj = new Consumer(i);
                Thread cons = new Thread(() => consumerObj.Run());
                CList.Add(cons);
                cons.Start();
            }
            Console.ReadLine();
            Console.WriteLine("=======");
            _flag = false;
            
            /*Clearing */
            for (int i = 0; i < _qCons; i++)
            {
                CList[0].Join();
                CList.RemoveAt(0);
            }
            for (int i = 0; i < _qProd; i++)
            {
                PList[0].Join();
                PList.RemoveAt(0);
            }
            Console.WriteLine("Press 'Enter' to continue...");
            Console.ReadLine();
        }

        public class Producer
        {
            private  int _id;

            public Producer(int Id)
            {
                _id = Id;

            }

            public void Run()
            {
                while (_flag)
                {
                    Lock.WaitOne();
                    _buf.Add(_id);
                    Console.WriteLine("Producer №{0} add {1}", _id, _id);
                    Lock.Release();
                    Thread.Sleep(_pause);
                }
            }
        }

        public class Consumer
        {
            private int _id;

            public Consumer(int Id)
            {
                _id = Id;

            }

            public void Run()
            {
                while (_flag)
                {
                    Lock.WaitOne();
                    if (_buf.Count() > 0)
                    {
                        int item = _buf.First();
                        if(_buf.Remove(item))
                          Console.WriteLine("Consumer №{0} get  {1}", _id, item);
                    }
                    Lock.Release();
                    Thread.Sleep(_pause);
                }
            }
        }
    }
}
