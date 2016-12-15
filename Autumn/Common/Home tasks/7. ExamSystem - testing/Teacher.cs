using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExamSystem
{
    class Teacher
    {
        private readonly int numAdd = 9;
        private readonly int numRemove = 1;
        private readonly int numContains = 90;
        private List<Tuple<long, long>> exams;
        private List<Tuple<long, long>> added;
        private Random rnd;
        private IExamSystem system;
        private Thread thread;
        private int name;

        public Teacher(int num, List <Tuple<long, long>> list, IExamSystem sys)
        {
            name = num;
            exams = list;
            added = new List<Tuple<long, long>>();
            rnd = new Random();
            system = sys;
            thread = new Thread(() => Run());
            thread.Start();
        }

        public void Run()
        {
            for (int i = 0; i < numAdd; i++)
            {
                int num = exams.Count;
                int student = rnd.Next() % num;
                Tuple<long, long> exam = exams[student];
                system.Add(exam.Item1, exam.Item2);
                added.Add(exam);
            }

            for (int i = 0; i < numContains; i++)
            {
                int num = exams.Count;
                int student = rnd.Next() % num;
                Tuple<long, long> exam = exams[student];
                bool check = system.Contains(exam.Item1, exam.Item2);
            }

            for (int i = 0; i < numRemove; i++)
            {
                int num = added.Count;
                int student = rnd.Next() % num;
                Tuple<long, long> exam = added[student];
                system.Remove(exam.Item1, exam.Item2);
                added.Remove(exam);
            }
        }

        public void ThreadJoin()
        {
            thread.Join();
        }
    }
}
