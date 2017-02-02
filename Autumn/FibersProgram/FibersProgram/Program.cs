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
            List<Fiber> listDifferentFibers = new List<Fiber>();
            List<Fiber> listFibers = new List<Fiber>();
            Console.Write("Enter num of fibers: ");
            int num = Convert.ToInt32(Console.ReadLine());
            Console.Write("Switch with priority? (Yes/No)  ");
            bool priority = ("Yes" == Console.ReadLine());
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
            ProcessManagerFramework.listFibers = listFibers;
            ProcessManagerFramework.priority = priority;
            ProcessManagerFramework.Switch(false);
            Console.WriteLine("Complited");
            //for(int i=0; i<fiberIdArr.Length;i++)
            //    Console.WriteLine(fiberIdArr[i]);
            DeleteAllFibers(listDifferentFibers);

            Console.ReadLine();
        }

        static void DeleteAllFibers(List<Fiber> listDifferentFibers)
        {
            try
            {
                foreach (Fiber fiber in listDifferentFibers)
                    fiber.Delete();
                Console.WriteLine("All fibers were removed");
            }
            catch (Exception)
            {
                Console.WriteLine("List empty. All fibers were removed");

            }
        }
    }
}
