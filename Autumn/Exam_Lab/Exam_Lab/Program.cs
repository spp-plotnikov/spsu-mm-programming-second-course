using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Exam_Lab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("LockList: " + ExamTest.Time(new LockListExamSystem()).ToString() + " ms");
            Console.WriteLine("LazyList: " + ExamTest.Time(new LazyListExamSystem()).ToString() + " ms");
        }
    }
}
