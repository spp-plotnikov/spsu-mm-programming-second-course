using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            HashTable<int> example = new HashTable<int>();
            example.Add(1);
            example.Add(2);
            Console.WriteLine(example.Search(3).ToString());
            Console.WriteLine(example.Search(1).ToString());
            example.Delete(1);
            Console.WriteLine(example.Search(1).ToString());
            example.Delete(1);
            Console.WriteLine(example.Search(2).ToString());
            Console.ReadLine();
        }
    }
}
