using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public interface IExamSystem
{
    void Add(long studentId, long courseId);

    void Remove(long studentId, long courseId);

    bool Contains(long studentId, long courseId);
}

class Storage : IExamSystem
{
    private Dictionary<long, Dictionary<long, bool>> table = new Dictionary<long, Dictionary<long, bool>>();
    private Mutex locker = new Mutex();

    public void Add(long studentId, long courseId)
    {
        // entry critical section
        locker.WaitOne();
        
        // student already exist 
        if (table.ContainsKey(studentId))
            table[studentId].Add(courseId, true);
        else // otherwise  
            table.Add(studentId, new Dictionary<long, bool> { { courseId, true } });

        // leave crtial section
        locker.ReleaseMutex();
    }

    public void Remove(long studentId, long courseId)
    {
        locker.WaitOne();

        // check exist and remove 
        if (table.ContainsKey(studentId) && table[studentId].ContainsKey(courseId))
            table[studentId].Remove(courseId);

        locker.ReleaseMutex();
    }

    public bool Contains(long studentId, long courseId)
    {
        locker.WaitOne();

        bool exist = (table.ContainsKey(studentId) && table[studentId].ContainsKey(courseId));

        locker.ReleaseMutex();

        return exist;
    }

    public void Print()
    {
        foreach (KeyValuePair<long, Dictionary<long, bool>> kvp in table)
            foreach (KeyValuePair<long, bool> kvp1 in kvp.Value)
                Console.WriteLine("Key = " + kvp.Key + ", Value = " +  kvp1.Key + "");
    }
}


namespace ExamStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Storage st = new Storage();
            st.Add(1, 2);
            st.Add(1, 3);
            Console.WriteLine(st.Contains(1, 3));
            st.Remove(1, 3);
            Console.WriteLine(st.Contains(1, 3));

            st.Print();
            Console.ReadKey();
        }
    }
}
