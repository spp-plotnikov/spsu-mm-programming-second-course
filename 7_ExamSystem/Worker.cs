using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Worker
{
    private long[] Students;
    private long[] Courses;
    private IExamSystem Table;
    public Thread ThreadWorker;
    Random rnd = new Random();

    public Worker(long[] students, long[] courses, IExamSystem table)
    {
        this.Students = students;
        this.Courses = courses;
        this.Table = table;
        this.ThreadWorker = new Thread(() => Run());
        ThreadWorker.Start();
    }
    public void Run()
    {
        int j, k;
        for (int i = 0; i < 90; i++)
        {
            j = rnd.Next(50);
            k = rnd.Next(10);
            Table.Add(Students[j], Courses[k]);
        }
        for (int i = 0; i < 9; i++)
        {
            j = rnd.Next(50);
            k = rnd.Next(10);
            Table.Remove(Students[j], Courses[k]);
        }
        j = rnd.Next(50);
        k = rnd.Next(10);
        //Table.Contains(Students[j], Courses[k]);
        Console.WriteLine(Table.Contains(Students[j], Courses[k]));
    }
}
