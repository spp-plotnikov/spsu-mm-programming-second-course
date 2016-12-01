using System;

namespace Deanery
{
    class Program
    {
        static void Main(string[] args)
        {
            NotTrivialImplementation system = new NotTrivialImplementation();
            system.Add(1, 1);

            system.Add(1, 2);
            system.Add(1, 3);
            system.Add(2, 3);
            system.Add(2, 4);
            Console.WriteLine(system.Contains(1, 1));
            Console.WriteLine(system.Contains(1, 4));
            system.Remove(1, 1);
            Console.WriteLine(system.Contains(1, 1));
            Console.ReadKey();
        }
    }
}
