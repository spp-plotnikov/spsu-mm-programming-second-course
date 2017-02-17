using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<Action> list = new Queue<Action>();
            for(int i = 0; i < 11; i++)
            {
                list.Enqueue(() => Thread.Sleep(1000));
            }
            ThreadPool pool = new ThreadPool(list, 5);
        }
    }
}
