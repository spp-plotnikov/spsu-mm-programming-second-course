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
using System.Drawing;


namespace overloadTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // make imgs
            /*var tester = new Program();
            tester.makeDifferentImages("../../test.jpg", 40);*/
            
            Thread.Sleep(1000);

            string[] imgsPaths = Directory.GetFiles("../../images");
            int imgNum = imgsPaths.Length;

            double[] time = new double[imgNum];

            for (int j = 0; j < imgNum; ++j)
            {
                string curImgName = Path.GetFileNameWithoutExtension(imgsPaths[j]);
                Console.WriteLine("Start testing on " + curImgName + " img");
               
                var tester = new Program();
               
                double workTime = Math.Round((double)(tester.send(imgsPaths[j]) / 1000.0), 3);
                time[j] = workTime;
                Console.WriteLine("Testing finish on " + curImgName + " img, work time: " + workTime);

                Thread.Sleep(100);
            }
            
            for (int i = 0; i < time.Length; ++i)
            {
                StreamWriter file = File.AppendText("../../workTime.txt");
                file.WriteLine(time[i]);
                file.Close();

                double[] subTime = time.SubArray(0, i + 1);
               
                Array.Sort(subTime);

                double sum = 0;
                for (int j = 0; j < subTime.Length; ++j)
                {
                    Console.Write(subTime[j] + " ");
                    sum += subTime[j];
                }

                double middle = Math.Round(sum / (double)subTime.Length, 3);
                double median = subTime[subTime.Length / 2];

                Console.WriteLine("middle: " + middle + "; median: " + median);

                file = File.AppendText("../../middle.txt");
                file.WriteLine(middle);
                file.Close();

                file = File.AppendText("../../median.txt");
                file.WriteLine(median);
                file.Close();
            }



            Console.WriteLine("Done!");  
            Console.ReadKey();
        }
        

        private void makeDifferentImages(string path, int numOfImages)
        {
            Bitmap source = new Bitmap(path);

            int wRatio = (int)((source.Width - 1000) / numOfImages);
            int hRatio = (int)((source.Height - 1000) / numOfImages);

            int curW = source.Width;
            int curH = source.Height;

            for (int i = 0; i < numOfImages; ++i)
            {
                curW -= wRatio;
                curH -= hRatio;
                Console.WriteLine(curW + "x" + curH);
                using (StreamWriter sw = File.AppendText("../../imgSizes.txt"))
                {
                    sw.WriteLine(curW * curH);
                }
                CropImg(path, curW, curH);
            }
        }

        private void CropImg(string path, int newWidth, int newHeight)
        {
            Bitmap source = new Bitmap(path);
           
            var section = new Rectangle(new Point(0, 0), new Size(newWidth, newHeight));

            var CroppedImage = new Bitmap(section.Width, section.Height);

            var g = Graphics.FromImage(CroppedImage);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            var stream = new MemoryStream();
            
            CroppedImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);

            string newPath = "../../images/" + newWidth.ToString() + "x" + newHeight.ToString() + ".base64";
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                sw.WriteLine("data:image/jpeg;base64," + Convert.ToBase64String(stream.ToArray()));
            }
        }
        
        private int send(string path)
        {
            var ws = new WebSocket("ws://localhost:8089");
            ws.Log.Level = LogLevel.Error;
            

            string img = File.ReadAllText(path);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();           

            ws.Connect();
            ws.Send("{\"filter\":\"1\", \"img\":\"" + img + "\"}");

            int time = 0;
            ManualResetEvent oSignalEvent = new ManualResetEvent(false);

            ws.OnClose += (sender, e) =>
            {
                TimeSpan ts = stopWatch.Elapsed;
                time = Convert.ToInt32(Math.Round(ts.TotalMilliseconds / 1000, 3).ToString().Replace(",", "")); 
                oSignalEvent.Set();
            };

            oSignalEvent.WaitOne(); 
            oSignalEvent.Reset();

            return time;
        }

    }
}
