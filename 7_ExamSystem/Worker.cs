using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Worker
{
    private long[] students;
    private long[] courses;
    private IExamSystem table;
    public Thread ThreadWorker;
    private Random rnd = new Random();

    public Worker(long[] students, long[] courses, IExamSystem table)
    {
        this.students = students;
        this.courses = courses;
        this.table = table;
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
            table.Add(students[j], courses[k]);
        }
        for (int i = 0; i < 9; i++)
        {
            j = rnd.Next(50);
            k = rnd.Next(10);
            table.Remove(students[j], courses[k]);
        }
        j = rnd.Next(50);
        k = rnd.Next(10);
        //Table.Contains(Students[j], Courses[k]);
        Console.WriteLine(table.Contains(students[j], courses[k]));
    }
}
