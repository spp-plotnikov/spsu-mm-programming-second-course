using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystems
{
    class Bots : IDisposable
    {
        IExamSystem organisation;
        bool flag = true;
        List<Thread> hum = new List<Thread>();
        int d = 0; //for random
        Random r = new Random(0);
        int[] iters;
        public Bots(int number, bool easyType)
        {
            if(easyType) organisation = new BucketOrganisation();
            else organisation = new EasyOrganisation();
            iters = new int[number];
            for (int i = 0; i < number; i++)
            {                
                d = i;
                hum.Add(new Thread(StartWork));
                iters[i] = 0;              
                hum[i].Name = i.ToString();
                hum[i].Start();
            }
        }

        void StartWork()
        {
            int n = Convert.ToInt32(Thread.CurrentThread.Name);
            while (flag)
            {
                iters[n]++;
                int randNum = r.Next() % 100 + d;
                //90% всех вызовов – Contains, 9% - Add, 1% - Remove
                int st = r.Next(0, 10) + d * 3;
                int c = r.Next(1, 5);
                bool contains;
                if (randNum <= 90)
                {
                   // Console.WriteLine("Bot_" + "{0} find element ({1},{2}): {3}", Thread.CurrentThread.Name, st, c, organisation.Contains(st, c));
                    contains = organisation.Contains(st, c);
                }
                else if (randNum <= 99)
                {
                    //Console.WriteLine("{0} add element ({1},{2})", Thread.CurrentThread.Name, st, c);
                    organisation.Add(st, c);
                }
                else
                {
                    //Console.WriteLine("{0} remove element ({1},{2})", Thread.CurrentThread.Name, st, c);
                    organisation.Remove(st, c);
                }               
                Thread.Sleep(0);
            }

        }

        public void Dispose()
        {
            flag = false;
            foreach (var item in hum)
            {
                item.Join();
            }
            Console.WriteLine(" {0} iterations", iters.Sum());
        }
    }
}
