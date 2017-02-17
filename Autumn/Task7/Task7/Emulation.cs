using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task7
{
    public class Emulation
    {
        private IExamSystem system;
        private int teacherNumber;
        private List<Teacher> teachers;
        public int Result
        {
            get;
            private set;
        }

        public Emulation(IExamSystem system, int teacherNumber)
        {
            this.system = system;
            this.teacherNumber = teacherNumber > 0 ? teacherNumber : 5;
            teachers = new List<Teacher>();
            Result = -1;
        }

        public void Init()
        {
            for (int i = 0; i < teacherNumber; i++)
            {
                var info = new List<KeyValuePair<long, long>>();
                for (int j = 0; j < 1000; j++)
                {
                    info.Add(new KeyValuePair<long, long>((i + 1) * teacherNumber + j, new Random().Next((j + i + 1) * 5 % 100 + 1)));
                }
                Teacher teacher = new Teacher(info);
                teachers.Add(teacher);
            }
        }

        public void Start()
        {
            Stopwatch s = new Stopwatch();
            s.Reset();
            s.Start();
            List<Task> tasks = new List<Task>();
            for(int i = 0; i < teacherNumber; i++)
            {
                int index = i;
                tasks.Add(Task.Run(() => teachers[index].Work(system)));
            }
            Task.WaitAll(tasks.ToArray());
            Result = (int)s.ElapsedMilliseconds;
            s.Stop();
        }
    }
}
