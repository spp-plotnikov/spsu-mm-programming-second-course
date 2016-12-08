using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

class Benchmark
{
    private const int numOfAdd = 900;
    private const int numOfContains = 9000;
    private const int numOfRemove = 100;

    int[] distrByNums = new int[] { numOfAdd, numOfContains, numOfRemove };

    private const int numOfThreads = 14;
    private int[] distrByThreads = new int[] { 2, 10, 2 }; // add, contain, remove

    private const int studentIdLowerBound = 10;
    private const int studentIdUpperBound = 10000;

    private const int courseIdLowerBound = 1;
    private const int courseIdUpperBound = 100;

    private KeyValuePair<long, long>[] containsSet, addSet, removeSet;

    private IExamSystem storage;

    public Benchmark(IExamSystem storage)
    {
        this.containsSet = new KeyValuePair<long, long>[numOfContains];
        this.addSet = new KeyValuePair<long, long>[numOfAdd];
        this.removeSet = new KeyValuePair<long, long>[numOfRemove];
        this.storage = storage;

        Random rnd = new Random();

        for (int i = 0; i < numOfContains; ++i)
            containsSet[i] = new KeyValuePair<long, long>(rnd.Next(studentIdLowerBound, studentIdUpperBound), rnd.Next(courseIdLowerBound, courseIdUpperBound));

        for (int i = 0; i < numOfAdd; ++i)
            addSet[i] = new KeyValuePair<long, long>(rnd.Next(studentIdLowerBound, studentIdUpperBound), rnd.Next(courseIdLowerBound, courseIdUpperBound));

        KeyValuePair<long, long>[] shuffleAddSet = addSet.OrderBy(x => rnd.Next()).ToArray();
        Array.Copy(shuffleAddSet, 0, removeSet, 0, numOfRemove);
    }

    private void addThread(int begin, int end)
    {
        for (int i = begin; i <= end; ++i)
            storage.Add(addSet[i].Key, addSet[i].Value);
    }

    private void containThread(int begin, int end)
    {
        for (int i = begin; i <= end; ++i)
            storage.Contains(containsSet[i].Key, containsSet[i].Value);
    }

    private void removeThread(int begin, int end)
    {
        for (int i = begin; i <= end; ++i)
            storage.Remove(removeSet[i].Key, removeSet[i].Value);
    }



    public void StartTesting()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        int done = 0;

        for (int th = 0; th < 3; ++th)
        {
            for (int i = 0; i < distrByThreads[th]; ++i)
            {
                int left = i * (distrByNums[th] / distrByThreads[th]);
                int right = left + (distrByNums[th] / distrByThreads[th]) - 1;
                if (th == 0)
                {
                    Thread thread = new Thread(() =>
                    {
                        addThread(left, right);
                        done++;
                    });
                    thread.Start();
                }
                else if (th == 1)
                {
                    Thread thread = new Thread(() =>
                    {
                        containThread(left, right);
                        done++;
                    });
                    thread.Start();
                }
                else if (th == 2)
                {
                    Thread thread = new Thread(() =>
                    {
                        removeThread(left, right);
                        done++;
                    });
                    thread.Start();
                }
            }
        }

        while (done < numOfThreads);

        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        Console.WriteLine("RunTime: " + elapsedTime);
    }
}