using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiltersTest_Lab
{
    class Program
    {

        static void Main(string[] args)
        {
            const int N = 10;
            List<Task<long>> tasks = new List<Task<long>>();
            for (int i = 0; i < N; i++)
            {
                tasks.Add(new Task<long>(() => Work()));
            }

            foreach (Task<long> task in tasks)
            {
                task.Start();
            }

            long time = 0;
            foreach (Task<long> task in tasks)
            {
                time += task.Result;
            }
            Console.WriteLine(N.ToString() + ": " + (time / N).ToString());


        }

        private static long Work ()
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
            string path = "C:/Test/test4.bmp";
            Stopwatch timer = new Stopwatch();
            timer.Start();
            client.Send(filter, path);
            timer.Stop();

            return timer.ElapsedMilliseconds;
        }
    }
}
