using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UniversityRequest
{
    public static class TimeCounter
    {
        public static long Count(IExamSystem system)
        {
            const int percent = 30;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<Task> tasks = new List<Task>();
            Random random = new Random();

            for (int i = 0; i < percent; i++)
            {
                tasks.Add(new Task(() => system.Add(random.Next(), random.Next())));
            }
            for (int i = 0; i < 9 * percent; i++)
            {
                tasks.Add(new Task(() => system.Remove(random.Next(), random.Next())));
            }
            for (int i = 0; i < 90 * percent; i++)
            {
                tasks.Add(new Task(() => system.Contains(random.Next(), random.Next())));
            }

            foreach (var task in tasks)
            {
                task.Start();
            }
            Task.WaitAll(tasks.ToArray());

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}