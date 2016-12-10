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
            IExamSystem examSystem1 = new LockExamSystem();
            User teacher1 = new User(examSystem1);
            User teacher2 = new User(examSystem1);
            teacher1.Pass(12, 1);
            Console.WriteLine(teacher2.IsPassed(12, 1).ToString());
            teacher2.Fail(12, 1);
            Thread.Sleep(10);
            Console.WriteLine(teacher1.IsPassed(12, 1).ToString());
            
            IExamSystem examSystem2 = new LazyListExamSystem();
            User teacher3 = new User(examSystem2);
            User teacher4 = new User(examSystem2);
            teacher3.Pass(12, 2);
            Console.WriteLine(teacher4.IsPassed(12, 2).ToString());
            teacher4.Fail(12, 2);
            Thread.Sleep(10);
            Console.WriteLine(teacher3.IsPassed(12, 2).ToString());
        }
    }
}
