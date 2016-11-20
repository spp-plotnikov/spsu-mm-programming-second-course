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
        static int[][] Pars(string namefile, ref List<int> notInQ)
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

            var arraySize = Convert.ToInt32(subStrings[0]);
            int[][] arr = new int[arraySize][];
            int i, j;

            for (i = 0; i < arraySize; i++)
            {
                notInQ.Add(i);
                arr[i] = new int[arraySize + 1];
                arr[i][arraySize] = i;
                for (j = 0; j < arraySize; j++)
                {
                    arr[i][j] = 0;
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
                    arr[j][i] = k;
                    arr[i][j] = k;
                }
            }
            return arr;
        }

        static int[] MyMin(int[][] array, List<int> inQ, List<int> notInQ)
        {
            int min = int.MaxValue;
            int line = -1;
            int col = -1;
            for (int i = 0; i < array.Length; i++)
            {
                foreach (var el in notInQ)
                {
                    //Console.Write(el + " ");
                    if (array[i][el] == 0) continue;
                    else
                    {
                        if (array[i][el] < min)
                        {
                            min = array[i][el];
                            col = el;
                            line = array[i][array[i].Length - 1];
                        }
                          
                    }
                }
                //Console.WriteLine();
            }
            return new[] { line, col, min };
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
                int[][] parsed = null;
                List<int> inQueue = new List<int>();
                List<int> notInQueue = new List<int>();
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
                stopWatch.Start();
                inQueue.Add(0);
                notInQueue.Remove(0);

                int[] minNow = MyMin(new int[][] { parsed[0]}, inQueue, notInQueue);

                //Console.WriteLine("MinNow: " + minNow[0] + " " + minNow[1] + " " + minNow[2]);

                totalLength += minNow[2];

                //Console.WriteLine("tl: " + totalLength);

                inQueue.Add(minNow[1]);
                notInQueue.Remove(minNow[1]);

                int[][][] thr = new int[comm.Size][][];
                int queLen = inQueue.Count();
                while (notInQueue.Any())
                {
                    if (comm.Rank == 0)
                    {
                        //Console.Write("inQ: ");
                        //foreach (var q in inQueue)
                        //{
                        //    Console.Write(q + " ");
                        //}
                        //Console.WriteLine("\n");

                        #region 
                       // queLen = inQueue.Count();
                        if (queLen <= comm.Size)
                        {
                            for (int i = 0; i < queLen; i++)
                            {
                                thr[i] = new int[][] { parsed[inQueue[i]] };
                            }

                            for (int i = queLen - 1; i < comm.Size; i++)
                            {
                                thr[i] = new int[][] { parsed[inQueue[0]] };
                            }
                        }
                        else
                        {
                            int delta = queLen / comm.Size;
                            //Console.WriteLine("delta: " + delta);
                            for (int i = 0; i < comm.Size; i++)
                            {
                                if (i == comm.Size - 1)
                                {
                                    int _d = delta + queLen % comm.Size;
                                    thr[i] = new int[_d][];

                                    for (int j = 0; j < _d; j++)
                                    {
                                        //Console.WriteLine("del + _d; inQueue[j + delta * i]: " + inQueue[j + delta * i]);
                                        thr[i][j] = parsed[inQueue[j + delta * i]];
                                    }
                                }
                                else
                                {
                                    thr[i] = new int[delta][];

                                    for (int j = 0; j < delta; j++)
                                    {
                                        //Console.WriteLine(i + ": inQueue[j + delta * i]: " + inQueue[j + delta * i]);
                                        thr[i][j] = parsed[inQueue[j + delta * i]];
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    int[][] x = comm.Scatter(thr, 0); //start parall

                    int[] minHere = MyMin(x, inQueue, notInQueue);

                    //Console.Write("min on " + comm.Rank + " is ");
                    //foreach (var t in minHere)
                    //{
                    //    Console.Write(t + " ");
                    //}
                    //Console.Write("\n");

                    int[][] preMins = comm.Gather(minHere, 0); //finish parall


                    //if (comm.Rank == 0)
                    //{
                    //    Console.Write("preMin: ");
                    //    foreach (var t in preMins)
                    //    {
                    //        Console.Write(t[2] + ", ");
                    //    }
                    //    Console.Write("\n");
                    //}


                    if (comm.Rank == 0)
                    {
                        int _min = preMins[0][2];
                        int[] _preMin = preMins[0];

                        for (int i = 1; i < preMins.Length; i++)
                        {
                            if (_min > preMins[i][2])
                            {
                                _min = preMins[i][2];
                                _preMin = preMins[i];
                            }
                        }

                        totalLength += _min;
                        inQueue.Add(_preMin[1]);
                        notInQueue.Remove(_preMin[1]);
                        queLen++;
                        if (queLen % 100 == 0)
                        {
                            notInQueue.Sort();
                            Console.WriteLine("Qlen: " + queLen + "; tl: " + totalLength);
                        }
                    }

                    comm.Broadcast(ref notInQueue, 0);
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
