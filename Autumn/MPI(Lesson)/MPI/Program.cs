using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPI
{
    class Program
    {
        static void Main(string[] args)
        {
            int sum = 0;
            List<Thread> threads = new List<Thread>();

            for(int i = 0; i<4; i++)
            {
                var t = new Thread(
                    () =>
                    {
                        for (int j = 0; j < 100000; j++)
                            sum += 1;
                    });
                t.Start();
                threads.Add(t);
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }
            Console.WriteLine(sum);
            Console.ReadKey();
        }
            /*static void Main(string[] args)
            {
                using (var env = new MPI.Environment(ref args))
                {
                    var world = Communicator.world;

                    if(world.Rank == 0)
                    {

                        var lst = new List<int>() { 0 };
                        for(int i=1;i<world.Size; i++)
                        {
                            world.Send<int>(i * 10, i, 42);
                            lst.Add(i * 100);
                            //Console.ReadKey();
                        }

                        var result = world.Scatter<int>(((IEnumerable<int>)lst).ToArray(), 0);  //нач версия без интерфейса
                        //Console.WriteLine(result);

                    }
                    else
                    {
                        var result = world.Receive<int>(0, 42);
                        Console.WriteLine(result);

                        result = world.Scatter<int>(0);

                        Console.WriteLine(result);
                    }

                    int[] arr = new int[0];
                    if (world.Rank == 0)
                    {
                        arr = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
                    }
                    world.Broadcast<int[]>(ref arr, 0);
                    Console.WriteLine(arr[7]);

                    var totalResult = world.Reduce<int>(0, (x, y) => x + y, 0);

                    Console.WriteLine("{0} {1}", world.Rank, totalResult);
                }
            }*/

        public class SampleLock
        {
            private bool[] flag = new bool[2];

            public void Lock()
            {
                int i = ThreadID.get();
                int j = 1 - i;
                flag[i] = true;
                while (flag[j]) { }  //wait

            }

            public void Unlock()
            {
                int i = ThreadID.get();
                flag[i] = false;
            }
        }
        }
}
