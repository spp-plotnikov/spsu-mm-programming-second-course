using MPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prim2
{
    class Program
    {
        static int arraySize;
        static int[,] parsed = null;
        static List<int>[] inQueue;
        static List<int> notInQueue;

        static int[,] Pars(string namefile, ref List<int> notInQ)
        {
            StreamReader file = new StreamReader(@namefile);
            var line = file.ReadLine();
            if (line == null) return null;

            string[] subStrings = line.Split(' ');
            if (subStrings.Length != 1)
            {
                Console.WriteLine("Error format");
                return null;
            }

            arraySize = Convert.ToInt32(subStrings[0]);
            int[,] arr = new int[arraySize, arraySize + 1];
            int i, j;

            for (i = 0; i < arraySize; i++)
            {
                notInQ.Add(i);
                for (j = 0; j < arraySize; j++)
                {
                    arr[i,j] = 0;
                }
            }

            while ((line = file.ReadLine()) != null)
            {
                subStrings = line.Split(' ');

                if (subStrings.Length != 3)
                {
                    Console.WriteLine("Error format: incorrect number of argumets in line");
                    return null;
                }
                else
                {
                    i = Convert.ToInt32(subStrings[0]);
                    j = Convert.ToInt32(subStrings[1]);
                    if (i >= j)
                    {
                        Console.WriteLine("Error format: i >= j");
                        return null;
                    }
                    int k = Convert.ToInt32(subStrings[2]);
                    arr[j, i] = k;
                    arr[i, j] = k;
                }
            }
            return arr;
        }

        static int[] MyMin(int num)
        {
            int min = int.MaxValue;
            int line = -1;
            int col = -1;
            foreach(int i in inQueue[num])
            {
                foreach (var el in notInQueue)
                {
                    //Console.Write(el + " ");
                    if (parsed[i, el] == 0) continue;
                    else
                    {
                        if (parsed[i, el] < min)
                        {
                            min = parsed[i, el];
                            col = el;
                            line = parsed[i, arraySize];
                        }
                          
                    }
                }
                //Console.WriteLine();
            }
            return new[] { line, col, min };
        }

        public static int[] TotMin(int[] m1, int[] m2)
        {
            if (m1[2] < m2[2])
            {
                return m1;
            }
            else
            {
                return m2;
            }
        }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                #region 
                if (args.Count() != 2)
                {
                    Console.WriteLine("Not enough parametrs. Add input and output files");
                    return;
                }

                string pathIn = "";
                string pathOut = "";
                notInQueue = new List<int>();
                int totalLength = 0;

                pathIn = args[0];
                pathOut = args[1];
                if (!File.Exists(pathIn))
                {
                    Console.WriteLine("Nonexistent file. Try again:");
                    return;
                }

                parsed = Pars(pathIn, ref notInQueue);

                if (parsed == null)
                {
                    Console.WriteLine("NULL");
                    return;
                }
                #endregion 
                Console.WriteLine("parsed");
                Stopwatch stopWatch = new Stopwatch();
                inQueue = new List<int>[comm.Size];

                for (int i = 0; i < comm.Size; i++)
                {
                    inQueue[i] = new List<int>();
                }

                stopWatch.Start();
                inQueue[0].Add(0);
                notInQueue.Remove(0);

                int[] minNow = MyMin(0);

                //Console.WriteLine("MinNow: " + minNow[0] + " " + minNow[1] + " " + minNow[2]);

                totalLength += minNow[2];

                //Console.WriteLine("tl: " + totalLength);

                inQueue[1 % comm.Size].Add(minNow[1]);
                notInQueue.Remove(minNow[1]);
               

                int queLen = inQueue.Count();

               
                while (notInQueue.Any())
                {
                    
                    int[] minHere = MyMin(comm.Rank);

                    //Console.Write("min on " + comm.Rank + " is ");
                    //foreach (var t in minHere)
                    //{
                    //    Console.Write(t + " ");
                    //}
                    //Console.Write("\n");


                    int[] _min = comm.Reduce(minHere, TotMin, 0); //finish parall
                    if (comm.Rank == 0)
                    {
                        totalLength += _min[2];
                        inQueue[queLen % comm.Size].Add(_min[1]);
                        notInQueue.Remove(_min[1]);
                        queLen++;                      

                        if (queLen % 100 == 0)
                        {
                            //notInQueue.Sort();
                            Console.WriteLine("Qlen: " + queLen + "; tl: " + totalLength);
                        }
                    }                   

                    comm.Broadcast(ref notInQueue, 0);
                    comm.Broadcast(ref inQueue, 0);
                }
            
                stopWatch.Stop();

                if (comm.Rank == 0)
                {
                    Console.Write("totalqueue: ");

                    string st = Convert.ToString(inQueue.Count()) + System.Environment.NewLine;
                    File.WriteAllText(@pathOut, st + totalLength, Encoding.Unicode);

                    Console.WriteLine(st + totalLength);
                }
                Console.WriteLine("time:" + (stopWatch.ElapsedMilliseconds / (1000.0d * 60.0d)));            
            }
        }
    }
}
