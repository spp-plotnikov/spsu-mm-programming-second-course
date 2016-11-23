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
        static void Main(string[] args)
        {
            Execute newProgram = new Execute();
            newProgram.Run();
            return;
        }        
    }

    public class Execute
    {


        const int numOfProd = 3;
        const int numOfCons = 3;

        public int wait = 1000;
        public int isLocked = 0; //0 - free
        public bool isEnd = false;

        public List<int> sharedList = new List<int>();
        public List<Producer> currentProducer = new List<Producer>();
        public List<Consumer> currentConsumer = new List<Consumer>();

        public void Run()
        {
            Console.WriteLine("press any key for end:");


            for (int i = 0; i < numOfProd; i++)
            {
                Producer prod = new Producer(i + 1, this);
                currentProducer.Add(prod);
            }

            for (int i = 0; i < numOfCons; i++)
            {
                Consumer cons = new Consumer(i + 1, this);
                currentConsumer.Add(cons);
            }
            Console.ReadKey();

            while (0 != Interlocked.CompareExchange(ref isLocked, 1, 0))
            { }

            Console.WriteLine("that's all");

            isEnd = true;

            for (int i = 0; i < numOfCons; i++)
            {
                currentConsumer[0].Delete();
                currentConsumer.RemoveAt(0);
            }
            for (int i = 0; i < numOfProd; i++)
            {
                currentProducer[0].Delete();
                currentProducer.RemoveAt(0);
            }

        }
    }

    public class Producer
    {
        int name;
        Thread myThread;
        public Producer(int name, Execute NewProgram)
        {
            this.name = name;
            myThread = new Thread(() => this.Run(NewProgram));
            myThread.Start();
        }

        public void Delete()
        {
            myThread.Join();
        }

        public void Run(Execute NewProgram)
        {
            while (!NewProgram.isEnd)
            {
                while (0 != Interlocked.CompareExchange(ref NewProgram.isLocked, 1, 0))
                { }
                Random rand = new Random();
                int val = rand.Next(100);
                NewProgram.sharedList.Add(val);
                Console.WriteLine("Producer # {0} add number {1} to list", name, val);
                Interlocked.Exchange(ref NewProgram.isLocked, 0);
                Thread.Sleep(NewProgram.wait);
            }
        }

    }

    public class Consumer
    {
        int name;
        Thread myThread;

        public Consumer(int name, Execute NewProgram)
        {
            this.name = name;
            myThread = new Thread(() => this.Run(NewProgram));
            myThread.Start();
        }


        public void Delete()
        {
            myThread.Join();
        }

        public void Run(Execute NewProgram)
        {
            while (!NewProgram.isEnd)
            {
                while (0 != Interlocked.CompareExchange(ref NewProgram.isLocked, 1, 0))
                { }
                if (NewProgram.sharedList.Count == 0)
                {
                    Console.WriteLine("Consumer # {0} can't get number because list is empty", name);
                }
                else
                {
                    int val = NewProgram.sharedList.Last();
                    Console.WriteLine("Consumer # {0} get {1} from list", name, val);
                    NewProgram.sharedList.Remove(val);
                }
                Interlocked.Exchange(ref NewProgram.isLocked, 0);
                Thread.Sleep(NewProgram.wait);
            }
        }
    }

}
