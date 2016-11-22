using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {

        public const int numOfProd = 3;
        public const int numOfCons = 3;
        public const int Wait = 1000;

        public static int IsLocked = 0; //0 - free
        public static bool IsEnd = false;

        public static List<int> SharedList = new List<int>();
        public static List<Thread> CurrentProducer = new List<Thread>();
        public static List<Thread> CurrentConsumer = new List<Thread>();

        static void Main(string[] args)
        {
            Console.WriteLine("press any key for end:");
            

            for (int i = 0; i < numOfProd; i++)
            {
                Producer prod = new Producer(i + 1);
                Thread newThread = new Thread(() => prod.Run());
                CurrentProducer.Add(newThread);
                newThread.Start();
            }

            for (int i = 0; i < numOfCons; i++)
            {
                Consumer cons = new Consumer(i + 1);
                Thread newThread = new Thread(() => cons.Run());
                CurrentConsumer.Add(newThread);
                newThread.Start();
            }
            Console.ReadKey();

            while (0 != Interlocked.CompareExchange(ref IsLocked, 1, 0))
            { }

            Console.WriteLine("that's all");

            IsEnd = true;

            for (int i = 0; i < numOfCons; i++)
            {
                CurrentConsumer[0].Join();
                CurrentConsumer.RemoveAt(0);
            }
            for (int i = 0; i < numOfProd; i++)
            {  
                CurrentProducer[0].Join();
                CurrentProducer.RemoveAt(0);
            }

            return;
        }


    }

    class Producer
    {
        int name;

        public Producer(int name)
        {
            this.name = name;
        }

        public void Run()
        {
            while (!Program.IsEnd)
            {
                while (0 != Interlocked.CompareExchange(ref Program.IsLocked, 1, 0))
                { }
                Random rand = new Random();
                int val = rand.Next(100);
                Program.SharedList.Add(val);
                Console.WriteLine("Producer # {0} add number {1} to list", name, val);
                Interlocked.Exchange(ref Program.IsLocked, 0);
                Thread.Sleep(Program.Wait);
            }
        }

    }

    class Consumer
    {
        int name;

        public Consumer(int name)
        {
            this.name = name;
        }

        public void Run()
        {
            while (!Program.IsEnd)
            {
                while (0 != Interlocked.CompareExchange(ref Program.IsLocked, 1, 0))
                { }
                if (Program.SharedList.Count == 0)
                {
                    Console.WriteLine("Consumer # {0} can't get number because list is empty", name);
                }
                else
                {
                    int val = Program.SharedList.Last();
                    Console.WriteLine("Consumer # {0} get {1} from list", name, val);
                    Program.SharedList.Remove(val);
                }
                Interlocked.Exchange(ref Program.IsLocked, 0);
                Thread.Sleep(Program.Wait);
            }
        }
    }

}
