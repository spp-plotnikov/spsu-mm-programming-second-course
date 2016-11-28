using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class ExamSystem
{
    public static long GetTime(long[] students, long[] courses, IExamSystem table)
    {
        var counter = System.Diagnostics.Stopwatch.StartNew();
        List<Worker> workers = new List<Worker>();
        for (int i = 0; i < 5; i++)
        {
            workers.Add(new Worker(students, courses, table));
        }
        foreach(Worker worker in workers)
        {
            worker.ThreadWorker.Join();
        }
        counter.Stop();
        return counter.ElapsedMilliseconds;
    }
    
    static void Main(string[] args)
    {
        long[] students = new long[50];
        long[] courses = new long[10];
        for(long i = 0; i < 50; i++)
        {
            students[i] = i;
        }
        for(long i = 0; i < 10; i++)
        {
            courses[i] = i;
        }
        long timeFirst = GetTime(students, courses, new SystemFirst());
        long timeSecond = GetTime(students, courses, new SystemSecond());
        Console.WriteLine("the first system = {0}", timeFirst);
        Console.WriteLine("the second system = {0}", timeSecond);
        Console.ReadLine();
    }
}
