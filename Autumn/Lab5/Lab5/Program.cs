using System;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            int capacity = rnd.Next(1000, 5000);
            int[] data = new int[capacity];
            for (int i = 0; i < capacity; i++)
            {
                data[i] = rnd.Next(500);
            }
            Console.WriteLine("First sum: " + new ParticularSum().Sum(data));
            Console.WriteLine("Second sum: " + new RecursiveSum().Sum(data));
            Console.Read();
        }
    }
}
