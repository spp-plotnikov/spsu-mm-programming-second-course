using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int clientsNum = 1;
            List<Test> clients = new List<Test>();
            List<KeyValuePair<int, long>> result = new List<KeyValuePair<int, long>>();
            Bitmap image = new Bitmap("test.bmp");
            byte[] bytes = (byte[])(new ImageConverter().ConvertTo(image, typeof(byte[])));
            while (true)
            {
                try
                {
                    clients.Clear();
                    List<Task<long>> procs = new List<Task<long>>();
                    for (int i = 0; i < clientsNum; i++)
                    {
                        clients.Add(new Test());
                    }
                    foreach (var client in clients)
                    {
                        procs.Add(Task.Run(() => client.Testing(bytes)));
                    }
                    Task.WaitAll(procs.ToArray());
                    foreach (var proc in procs)
                    {
                        if (proc.Result == -1)
                        {
                            Console.WriteLine("No!");
                            throw new NotImplementedException();
                        }
                    }
                    long time = 0;
                    Console.WriteLine(clientsNum);
                    procs.ForEach(x => time += x.Result);
                    result.Add(new KeyValuePair<int, long>(clientsNum, time));
                    clientsNum++;
                }
                catch
                {
                    break;
                }
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter("ClientTest.txt"))
            {
                foreach (var pair in result)
                {
                    file.WriteLine(pair.Key + " " + pair.Value / pair.Key);
                }
            }
            Console.WriteLine("Success!");
            Console.ReadLine();
        }
    }
}
