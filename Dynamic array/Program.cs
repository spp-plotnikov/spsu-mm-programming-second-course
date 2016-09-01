using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrayList
{
    class Program
    {
        public static void Print(System.Collections.IEnumerable obj)
        {
            foreach (var item in obj)
                Console.Write(item + " ");
            Console.WriteLine();
        }
        static void Main()
        {
            ArrayList<int> arr = new ArrayList<int>();
            Console.WriteLine("Add 1, 2, 3, 4");
            arr.Add(1);
            arr.Add(2);
            arr.Add(3);
            arr.Add(4);
            Print(arr);
            Console.WriteLine("Add 5 on index 2");
            arr.Insert(5, 2);
            Print(arr);
            Console.WriteLine("Remove element on index 2, 0");
            arr.RemoveAt(2);
            arr.RemoveAt(0);
            Print(arr);
            Console.WriteLine("Remove element 2");
            arr.Remove(2);
            Print(arr);

            if (arr.IndexOf(3) != -1)
                Console.WriteLine("Index of element '3' : " + arr.IndexOf(3));
            else
                Console.WriteLine("Not found");

            if (arr.IndexOf(5) != -1)
                Console.WriteLine("Index of element '5' : " + arr.IndexOf(5));
            else
                Console.WriteLine("Index of element '5' : Not found");
        }
    }
}
