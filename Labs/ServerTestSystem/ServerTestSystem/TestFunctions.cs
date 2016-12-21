using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTestSystem
{
    class TestFunctions
    {
        public int MaxSizeOfClients = 1;
        List<string> pict = new List<string>();
        public TestFunctions()
        {
            StreamReader file = new StreamReader(@"PictList.txt");
            string line;
            while ((line = file.ReadLine()) != null)
            {
                pict.Add(line);
            }
        }
        //•	Построить график распределения времени выполнения запросов при фиксированном размере изображения.
        //•	Найти число клиентов, приводящее к отказу от обслуживания, для некоторого размера изображения.
        public void MaxClients()
        {
            int numb = 0;
            List<Client> clients = new List<Client>();
            List<Task<float>> tasks = new List<Task<float>>();
            List<float> time = new List<float>();

            while (true)
            {
                numb++;
                clients.Add(new Client(numb));
                foreach (var cl in clients)
                {
                    tasks.Add(Task.Run(() => cl.SendPicture("pict/flower128.jpg")));
                }
                Task.WaitAll(tasks.ToArray());
                foreach (var t in tasks)
                {
                    if (t.Result == -1)
                    {
                        MaxSizeOfClients = numb;
                        return;
                    }
                    else
                    {
                        time.Add(t.Result);
                    }
                }

                File.AppendAllText(@"results.txt", numb.ToString() + " " + 
                                  (time.Sum() / numb).ToString() + Environment.NewLine,
                                  Encoding.Unicode);
                time.Clear();
                tasks.Clear();
            }
        }

        //•	Построить графики среднего и медианного времени выполнения запросов при различном общем числе пикселов в изображении.
        public void MediumAndMediana()
        {
            List<Client> clients = new List<Client>();
            List<Task<float>> tasks = new List<Task<float>>();
            List<float> time = new List<float>();
            int N = 4; //4 - число потоков в моем процессоре
            for (int i = 0; i < N; i++) 
            {
                clients.Add(new Client(i));
            }
            foreach (var p in pict)
            {
                StreamReader picture = new StreamReader(@p);
                var imageBitmap = new System.Drawing.Bitmap(picture.BaseStream);

                foreach (var cl in clients)
                {
                    tasks.Add(Task.Run(() => cl.SendPicture(p)));
                }
                Task.WaitAll(tasks.ToArray());
                foreach (var t in tasks)
                {                  
                    time.Add(t.Result);                    
                }
                time.Sort();
                File.AppendAllText(@"PictResults.txt", (imageBitmap.Width * imageBitmap.Height).ToString() +  " " +
                                   (time.Sum() / N).ToString() + " " + time[N / 2].ToString() + Environment.NewLine,
                                   Encoding.Unicode);
                time.Clear();
                tasks.Clear();
            }

        
        }
    }
}
