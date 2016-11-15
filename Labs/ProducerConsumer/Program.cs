using System;
using System.Collections.Generic;
using System.Threading;

namespace ProducerConsumer
{
    class Program
    {
        static List<Producer> pr = new List<Producer>();
        static List<Consumer> con = new List<Consumer>();

        static void Start()
        {
            int numbOfProd = 0, numbOfCons = 0;
            Console.WriteLine("Input numb of Producer: ");
            numbOfProd = Convert.ToInt32(Console.ReadLine());

            while (numbOfProd < 0)
            {
                Console.WriteLine("Not natural number. Input numb of Producers again: ");
                numbOfProd = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("Input numb of Consumer: ");
            numbOfCons = Convert.ToInt32(Console.ReadLine());
            while (numbOfCons < 0)
            {
                Console.WriteLine("Not natural number. Input numb of Consumers again: ");
                numbOfCons = Convert.ToInt32(Console.ReadLine());
            }
                      
            List<int> buff = new List<int>();
            Semaphore sem = new Semaphore(1, 1);

            for (int i = 0; i < numbOfCons; i++)
            {
                con.Add(new Consumer(ref buff, ref sem, i));
            }
            for (int i = 0; i < numbOfProd; i++)
            {
                pr.Add(new Producer(ref buff, ref sem, i));
            }

            Console.ReadKey();
            Console.WriteLine("All processes will be finished, please, wait");

            for (int i = 0; i < numbOfCons; i++)
            {
                con[0].Delete();
                con.RemoveAt(0);
            }
            for (int i = 0; i < numbOfProd; i++)
            {
                pr[0].Delete();
                pr.RemoveAt(0);
            }

            Console.WriteLine("The end");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            Start();
            return;
        }
    }
}
