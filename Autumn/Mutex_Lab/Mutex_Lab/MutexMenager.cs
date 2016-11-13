using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mutex_Lab
{
    public class MutexMenager
    {
        private static Mutex _mutex = new Mutex();
        private static bool _notFinish = true;
        private static List<int> Buf = new List<int>();
        private static int _pause = 100;

        public static void Emulation(int numOfProducers, int numOfConsuments)
        {
            Queue<Thread> prodThreadList = new Queue<Thread>();
            Queue<Thread> consThreadList = new Queue<Thread>();

            Console.WriteLine("Press smth to finish");
            for (int i = 0; i < numOfProducers; i++)
            {
                Producer producer = new Producer(i);
                Thread prodThread = new Thread(() => producer.Run());
                prodThreadList.Enqueue(prodThread);
                prodThread.Start();
            }
            for (int i = 0; i < numOfConsuments; i++)
            {
                Consumer consumer = new Consumer(i);
                Thread consThread = new Thread(() => consumer.Run());
                consThreadList.Enqueue(consThread);
                consThread.Start();
            }

            Console.ReadLine();
            _notFinish = false;

            for (int i = 0; i < numOfProducers; i++)
            {
                prodThreadList.Dequeue().Join();
            }
            for (int i = 0; i < numOfConsuments; i++)
            {
                consThreadList.Dequeue().Join();
            }

            Console.WriteLine("The end");
        }

        private class Producer
        {
            private int _id;

            public Producer(int id)
            {
                _id = id;
            }

            public void Run()
            {
                while (_notFinish)
                {
                    _mutex.WaitOne();
                    Buf.Add(_id);
                    Console.WriteLine("Put " + _id.ToString());
                    _mutex.ReleaseMutex();
                    Thread.Sleep(_pause);
                }
            }
        }
        private class Consumer
        {
            private int _id;

            public Consumer(int id)
            {
                _id = id;
            }

            public void Run()
            {
                while(_notFinish)
                {
                    _mutex.WaitOne();
                    if (Buf.Count > 0)
                    {
                        int prod = Buf.First();
                        Buf.Remove(prod);
                        Console.WriteLine(_id.ToString() + " get " + prod.ToString());
                    }
                    _mutex.ReleaseMutex();
                    Thread.Sleep(_pause);
                }
            }
        }
    }
}
