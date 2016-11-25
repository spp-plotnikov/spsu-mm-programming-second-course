using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

class ExamSystem
{
    public static long Time(Marks marks)
    {
        var counter = System.Diagnostics.Stopwatch.StartNew();
        List<Worker> workers = new List<Worker>();
        for (int i = 0; i < 30; i++)
        {
            workers.Add(new Worker(marks));
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
        long[] students = new long[500000];
        long[] courses = new long[1000];
        for(long i = 0; i < 500000; i++)
        {
            students[i] = i;
        }
        for(long i = 0; i < 1000; i++)
        {
            courses[i] = i;
        }
        Marks marksFirst = new Marks(students, courses, new SystemFirst());
        Marks marksSecond = new Marks(students, courses, new SystemSecond());
        long timeFirst = Time(marksFirst);
        long timeSecond = Time(marksSecond);
        Console.WriteLine("the first system = {0}", timeFirst);
        Console.WriteLine("the second system = {0}", timeSecond);
        Console.ReadLine();
    }
}
