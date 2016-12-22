using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientFilters
{
    static class Tests
    {
        private static Bitmap CropImage(Bitmap source, Rectangle section)
        {            
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bmp;
        }

        public static void SingleUserTest()
        {
            Thread.Sleep(2000);
            Bitmap srcImage = new Bitmap("Tower_Bridge_4K.jpg");
            var clientWork = new ClientWorkEmulator(srcImage, "Invert");
            do
            {
                StreamWriter fileTime = File.AppendText("test1.txt");
                StreamWriter fileSize = File.AppendText("test2.txt");
                var watch = Stopwatch.StartNew();
                clientWork.Start();
                watch.Stop();
                fileTime.WriteLine(watch.ElapsedMilliseconds);
                fileSize.WriteLine(srcImage.Width * srcImage.Height);
                srcImage = CropImage(srcImage, new Rectangle(new Point(0, 0), new Size(srcImage.Width - 50, srcImage.Height - 50)));
                fileTime.Close();
                fileSize.Close();
            } while (srcImage.Width > 60 && srcImage.Height > 60);
        }

        public static void MultipleUsersTest()
        { 
            Thread.Sleep(2000);
            var clientWork = new ClientWorkEmulator("testImage.jpg", "Invert");
            for (int numOfClients = 0; numOfClients < 200; numOfClients++)
            {
                StreamWriter file = File.AppendText("test.txt");
                List<Thread> threadList = new List<Thread>();
                for (int i = 0; i < numOfClients; i++)
                {
                    Thread thread = new Thread(clientWork.Start);
                    threadList.Add(thread);
                }
                var watch = Stopwatch.StartNew();
                foreach (Thread thread in threadList)
                {
                    thread.Start();
                }
                foreach (Thread thread in threadList)
                {
                    thread.Join();
                }
                watch.Stop();
                file.WriteLine(numOfClients + " " + watch.ElapsedMilliseconds);
                file.Close();
            }
            }
    }
}
