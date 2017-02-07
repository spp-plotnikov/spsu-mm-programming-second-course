using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FibersProgram
{
    class Program
    {

        static void Main(string[] args)
        {
            int num;
            string readLine;
            List<Fiber> listDifferentFibers = new List<Fiber>();
            List<Fiber> listFibers = new List<Fiber>();
            Console.Write("Enter num of fibers: ");
            while (true)
            {
                readLine = Console.ReadLine();
                try
                {
                    num = Convert.ToInt32(readLine);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Somthing wrong. Try again.");
                }
            }
            
            Console.Write("Switch with priority? (Yes/No)  ");
            bool priority = false;

            while (readLine != "Yes" && readLine != "No")
            {
                readLine = Console.ReadLine();
                if (readLine == "Yes" || readLine == "No")
                    priority = (readLine == "Yes");
                else
                    Console.WriteLine("Somthing wrong. Try again.");
            }

            for (int i = 1; i <= num; i++)
            {
                var proc = new Process(priority);
                var fiber = new Fiber(proc.Run);
                listFibers.Add(fiber);
                listDifferentFibers.Add(fiber);
                if (priority)
                    for (int j = 1; j <= proc.Priority; j++)
                    {
                        //Console.WriteLine(fiber.Id);
                        listFibers.Add(fiber);
                    }
            }
            //Console.ReadLine();

            ProcessManagerFramework.InitializeFunc(listFibers, listDifferentFibers, priority);
            ProcessManagerFramework.Switch(false);
            Console.WriteLine("Complited");
            //for(int i=0; i<fiberIdArr.Length;i++)
            //    Console.WriteLine(fiberIdArr[i]);
            ProcessManagerFramework.DeleteAllFibers();
            Console.ReadLine();
        }

        
    }
}
