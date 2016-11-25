using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class SystemFirst : IExamSystem
{
    private HashSet<Tuple<long, long>> Table;
    public SystemFirst()
    {
        this.Table = new HashSet<Tuple<long, long>>();
    }
    public void Add(long studentId, long courseId)
    {
        lock (Table)
        {
            Console.WriteLine("add {0} {1} {2}",studentId, courseId, Thread.CurrentThread.ManagedThreadId);
            Table.Add(Tuple.Create<long, long>(studentId, courseId));
        }
    }
    public bool Contains(long studentId, long courseId)
    {
        lock (Table)
        {
            Console.WriteLine("contains {0} {1}", studentId, courseId);
            return Table.Contains(Tuple.Create<long, long>(studentId, courseId));
        }
    }
    public void Remove(long studentId, long courseId)
    {
        lock (Table)
        {
            Console.WriteLine("remove {0} {1}", studentId, courseId);
            Table.Remove(Tuple.Create<long, long>(studentId, courseId));
        }
    }
}