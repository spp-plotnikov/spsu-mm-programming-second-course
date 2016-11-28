using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class SystemSecond : IExamSystem
{
    private ConcurrentCuckoo Table;

    public SystemSecond()
    {
        this.Table = new ConcurrentCuckoo(10);
    }
    public void Add(long studentId, long courseId)
    {
        Console.WriteLine("add {0} {1} {2}", studentId, courseId, Thread.CurrentThread.ManagedThreadId);
        Table.Add(Tuple.Create<long, long>(studentId, courseId));
    }
    public void Remove(long studentId, long courseId)
    {
        Console.WriteLine("remove {0} {1}", studentId, courseId);
        Table.Remove(Tuple.Create<long, long>(studentId, courseId));
    }
    public bool Contains(long studentId, long courseId)
    {
        Console.WriteLine("contains {0} {1}", studentId, courseId);
        return Table.Contains(Tuple.Create<long, long>(studentId, courseId));
    }
}
