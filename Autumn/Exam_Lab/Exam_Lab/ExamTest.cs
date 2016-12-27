using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Lab
{
    public static class ExamTest
    {
        public static long Time (IExamSystem examSystem)
        {
            const int onePercent = 20;
            List<Task> tasks = new List<Task>();
            Random rng = new Random();

            for (int i = 0; i < 10 * onePercent; i++)
            {
                tasks.Add(new Task(() => examSystem.Add(rng.Next(), rng.Next())));
            }
            for (int i = 0; i < onePercent; i++)
            {
                tasks.Add(new Task(() => examSystem.Remove(rng.Next(), rng.Next())));
            }
            for (int i = 0; i < 90 * onePercent; i++)
            {
                tasks.Add(new Task(() => examSystem.Contains(rng.Next(), rng.Next())));
            }

            Stopwatch timer = new Stopwatch();

            timer.Start();
            foreach (var task in tasks)
            {
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());
            timer.Stop();

            return timer.ElapsedMilliseconds;
        }
    }
}
