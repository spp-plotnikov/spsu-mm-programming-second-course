using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{
    class Humans: IDisposable
    {
        Org1 organisation = new Org1();
        //Org2 organisation = new Org2();
        bool flag = true;
        List<Thread> hum = new List<Thread>();
        int d = 0; //for random
        Random r = new Random(0);
        public Humans(int number)
        {
            for (int i = 0; i < number; i++)
            {
                d = i;
                hum.Add(new Thread(StartWork));
                hum[i].Name = "Human_" + i;
                hum[i].Start();
            }
        }

        void StartWork()
        {
            while (flag)
            {
                int randNum = r.Next() % 100 + d;
                //90% всех вызовов – Contains, 9% - Add, 1% - Remove
                int st = r.Next(0, 10) + d * 3;
                int c = r.Next(1, 5);
                if (randNum <= 90)
                {                    
                    Console.WriteLine("{0} find element ({1},{2}): {3}", Thread.CurrentThread.Name, st, c, organisation.Contains(st, c));

                }
                else if (randNum <= 99)
                {                    
                    Console.WriteLine("{0} add element ({1},{2})", Thread.CurrentThread.Name, st, c);
                    organisation.Add(st, c);
                }
                else
                {                    
                    Console.WriteLine("{0} remove element ({1},{2})", Thread.CurrentThread.Name, st, c);
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
        }
    }
}

