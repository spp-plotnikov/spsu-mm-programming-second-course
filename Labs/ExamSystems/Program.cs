using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystems
{
    class Program
    {    
        static void Main(string[] args)
        {
            Console.WriteLine("Input number of bots");
            int n = Convert.ToInt32(Console.ReadLine());
            Bots hum = new Bots(n);
            //Console.WriteLine("Press key to stop");
            //Console.ReadKey();            
            Thread.Sleep(5000); //
            hum.Dispose();
            Console.WriteLine("finita");
            Console.ReadKey();     
        }
    }
}
