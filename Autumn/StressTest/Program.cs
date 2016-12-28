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
            
            string[] fileList = Directory.GetFiles(@"..\img", "*.bmp");

            int N = fileList.Length;

            int q = 3;
            long[] res = new long[N];
            List<Task<long>>[] tasks = new List<Task<long>>[N];

            /* 2 */
            
            for(int i = 0; i < N; i++)
                for(int j  = 0; j < q; j++)
                  tasks[i] = new List<Task<long>>();


            for (int i = 0; i < N; i++)
                for (int j = 0; j < q; j++)
                {
                    int index = i;
                    tasks[i].Add(new Task<long>(() => Emulation(fileList[index])));
                }
           
            for (int i = 0; i < N; i++)
                foreach (var t in tasks[i])
                    t.Start();

            long time = 0;
            string res1 = "", res0 = "";
            for (int i = 0; i < N; i++)
            {
                long[] f = new long[q];
                time = 0;
                int c = 0;
                foreach (Task<long> task in tasks[i])
                {
                    time += task.Result;
                    f[c++] = task.Result;
                    //Console.WriteLine(task.Result);
                }

                Console.WriteLine("{0} {1}",128 * Math.Pow(2, i), time / N);
                Array.Sort(f);
                res1 += ((128 * Math.Pow(2, i)).ToString() +" " +(time / N).ToString() + Environment.NewLine);
                res0 += ((128 * Math.Pow(2, i)).ToString() + " " + (f[q / 2]).ToString() + Environment.NewLine);
            }
            File.WriteAllText(@"0.txt", res0);
            File.WriteAllText(@"1.txt", res1);


            /* 3 */
            int count = 0;
            string res2 = "";
            while (true)
            {
                count++;
                //Console.WriteLine("== {0} == ", count);

                Task<long>[] hammer = new Task<long>[count];
                for(int i = 0; i < count; i++)
                    hammer[i] = (new Task<long>(() => Emulation(fileList[1])));

                foreach (var t in hammer)
                    t.Start();

                long duration =  0;

                foreach (var t in hammer)
                    duration += t.Result;

                Console.WriteLine("{0} {1}", count, duration);
                res2 += (count.ToString() + " " +duration.ToString() + " "+ Environment.NewLine);
                File.WriteAllText(@"2.txt", res2);


            }
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
