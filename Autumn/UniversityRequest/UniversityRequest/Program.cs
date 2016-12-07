using System;
using System.Linq;
using UniversityRequest.ExamSystems;

namespace UniversityRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TimeCounter.Count(new SimpleSystem()));
            Console.WriteLine(TimeCounter.Count(new SmartSystem()));
            Console.ReadLine();
        }
    }
}
