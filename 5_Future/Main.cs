using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 public class Future
{
    private static int ArraySize = 99999;
    static void Main(string[] args)
    {
        int[] array = new int[ArraySize];
        for (int i = 0; i < ArraySize; i++)
        {
            array[i] = 1;
        }
        FirstSum first = new FirstSum();
        Console.WriteLine("first Sum = {0}", first.Sum(array));
        SecondSum second = new SecondSum();
        Console.WriteLine("secon Sum = {0}", second.Sum(array));
        Console.ReadLine();
    }
}
