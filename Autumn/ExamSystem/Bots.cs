using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{
    class Bots
    {
        IExamSystem _strategy;
        int[] _iter;
        Random _rand = new Random(0);
        List<Thread> _pool = new List<Thread>();

        public Bots(int num, IExamSystem organisation)
        {
            _strategy = organisation;

            _iter = new int[num];

            for (int i = 0; i < num; i++)
            {
                _pool.Add(new Thread(() => Emulation()));
                _pool[i].Name = i.ToString();
                _pool[i].Start();
            }

        }
        void Emulation()
        {
            int id = Int32.Parse(Thread.CurrentThread.Name);

            while (true)
            {
                _iter[id]++;
                int randRes = _rand.Next(0, 100);

                // 90 %  - contains
                if (randRes <= 90)
                    _strategy.Contains(_rand.Next(0, 30), _rand.Next(0, 6));
                // 9 % - Add
                else if (randRes <= 99)
                    _strategy.Add(_rand.Next(0, 30), _rand.Next(0, 6));
                else 
                    _strategy.Remove(_rand.Next(0, 30), _rand.Next(0, 6));

                //Thread.Sleep(10);

            }
        }


        public void StatInfo()
        {
            foreach (Thread th in _pool)
                th.Abort();

            Console.WriteLine(" {0} actions ", _iter.Sum());
        }
    }
}
