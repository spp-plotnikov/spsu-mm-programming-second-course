using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Net;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace overloadTest
{
    class Program
    {
        static void Main(string[] args)
        {

            for (int j = 1; j <= 200; ++j)
            {
                Console.WriteLine("Start testing on " + j + " users");
                var pool = new List<Thread>();
                int sumTime = 0;

                for (int i = 0; i < j; ++i)
                {
                    var thread = new Thread(() =>
                    {
                        var tester = new Program();
                        Interlocked.Add(ref sumTime, tester.test());
                    });
                    pool.Add(thread);
                    thread.Start();
                }
                for (int i = 0; i < j; ++i)
                    pool[i].Join();

                double middleTime = Math.Round((double)((sumTime / (double)j) / 1000.0), 3);
                Console.WriteLine("Testing finish on " + j + " users, middle time: " + middleTime);
                
                using (StreamWriter sw = File.AppendText("../../middleTime.txt"))
                {
                    sw.WriteLine(middleTime.ToString());
                }

                Thread.Sleep(100);
            }


            Console.WriteLine("Done!");  
            Console.ReadKey();
        }
        
        private int test()
        {
            var ws = new WebSocket("ws://localhost:8089");
            ws.Log.Level = LogLevel.Error;
            
            string img = File.ReadAllText("../../img.base64");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();           

            ws.Connect();
            ws.Send("{\"filter\":\"1\", \"img\":\"" + img + "\"}");

            int time = 0;
            ManualResetEvent oSignalEvent = new ManualResetEvent(false);

            ws.OnClose += (sender, e) =>
            {
                TimeSpan ts = stopWatch.Elapsed;
                time = Convert.ToInt32(Math.Round(ts.TotalMilliseconds / 1000, 3).ToString().Replace(",", "")); //Math.Round(ts.TotalMilliseconds / 1000, 3);
                oSignalEvent.Set();
            };

            oSignalEvent.WaitOne(); 
            oSignalEvent.Reset();

            return time;
        }

    }
}
