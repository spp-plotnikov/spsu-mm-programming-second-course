using System;
using System.Collections.Generic;
using System.Threading;

namespace Lab7
{
    class Program
    {
        static Random _rnd = new Random();
        static void Main(string[] args)
        {
            int capacity = _rnd.Next(5000, 20000);
            capacity -= capacity % 100;
            List<KeyValuePair<long, long>> listOfStudents = new List<KeyValuePair<long, long>>();
            Console.WriteLine("Number of students: {0}", capacity);

            for (int i = 0; i < capacity; i++)
            {
                listOfStudents.Add(new KeyValuePair<long, long>(_rnd.Next(), _rnd.Next()));
            }

            GetTimeWorking time = new GetTimeWorking(capacity, listOfStudents);

            Console.WriteLine("Simple implementation working time: {0}", time.FirstType());
            Console.WriteLine("Implementation with cuckoo hashing working time: {0}", time.SecondType());
            Console.Read();
        }
    }
}
