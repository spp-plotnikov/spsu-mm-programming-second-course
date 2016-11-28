using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class SystemSecond : IExamSystem
{
    private ConcurrentCuckoo table;

    public SystemSecond()
    {
        this.table = new ConcurrentCuckoo(10);
    }
    public void Add(long studentId, long courseId)
    {
        Console.WriteLine("add {0} {1} {2}", studentId, courseId, Thread.CurrentThread.ManagedThreadId);
        table.Add(Tuple.Create<long, long>(studentId, courseId));
    }
    public void Remove(long studentId, long courseId)
    {
        Console.WriteLine("remove {0} {1}", studentId, courseId);
        table.Remove(Tuple.Create<long, long>(studentId, courseId));
    }
    public bool Contains(long studentId, long courseId)
    {
        Console.WriteLine("contains {0} {1}", studentId, courseId);
        return table.Contains(Tuple.Create<long, long>(studentId, courseId));
    }
}
