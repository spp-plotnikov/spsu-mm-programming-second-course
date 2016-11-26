using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input number of people");
            int n = Convert.ToInt32(Console.ReadLine());
            Humans hum = new Humans(n);
            Console.WriteLine("Press key to stop");
            Console.ReadKey();
            hum.Dispose();
        }
    }
}
