using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Worker
{
    private Marks MyMarks;
    public Thread ThreadWorker;
    Random rnd = new Random();

    public Worker(Marks myMarks)
    {
        this.MyMarks = myMarks;
        this.ThreadWorker = new Thread(() => Run());
        ThreadWorker.Start();
    }
    public void Run()
    {
        int j, k;
        for (int i = 0; i < 90; i++)
        {
            j = rnd.Next(500000);
            k = rnd.Next(1000);
            MyMarks.Results.Add(MyMarks.Students[j], MyMarks.Courses[k]);
        }
        for (int i = 0; i < 9; i++)
        {
            j = rnd.Next(500000);
            k = rnd.Next(1000);
            MyMarks.Results.Remove(MyMarks.Students[j], MyMarks.Courses[k]);
        }
        j = rnd.Next(500000);
        k = rnd.Next(1000);
        MyMarks.Results.Contains(MyMarks.Students[j], MyMarks.Courses[k]);
    }
}
