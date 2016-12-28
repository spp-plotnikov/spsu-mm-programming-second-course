using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 3;
            long[] res = new long[N];
            List<Task<long>>[] tasks = new List<Task<long>>[N];

            /* 1 ?! */

            /* 2 */
            string[] fileList = Directory.GetFiles(@"..\img", "*.bmp");

            for(int i = 0; i < N; i++)
                tasks[i] = new List<Task<long>>();

            foreach (var path in fileList)
            {
                for (int i = 0; i < N; i++)
                    tasks[i].Add(new Task<long>(() => Emulation(path)));
            }
            for (int i = 0; i < N; i++)
                foreach (var t in tasks[i])
                    t.Start();
            long time = 0;
            for (int i = 0; i < N; i++)
            {
                time = 0;
                foreach (Task<long> task in tasks[i])
                {
                    time += task.Result;
                    Console.WriteLine(task.Result);
                }

                Console.WriteLine(">> {0}", time / N);
            }

            /* 3 */
        }

        private static long Emulation(string path)
        {
            List<string> FilterList = new List<string>();

            BmpClient client = new BmpClient("127.0.0.1:13000");

            try
            {
                string[] tmp = client.Init();
                foreach (string i in tmp)
                    FilterList.Add(i);
            }
            catch
            {
                Console.WriteLine("ConnectError");
            }

            string filter = FilterList[0];

            Stopwatch timer = new Stopwatch();
            timer.Start();
            client.Send(filter, path);
            timer.Stop();


            return timer.ElapsedMilliseconds;
        }
    }
}
