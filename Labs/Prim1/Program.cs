using System;
using System.Collections.Generic;
using MPI;
using System.IO;
using System.Linq;
using System.Text;

namespace Prim
{
    class Program
    {
        static int[][] Pars(string namefile)
        {
            StreamReader file = new StreamReader(@namefile);
            var line = file.ReadLine();
            if (line == null) return null;

            String[] subStrings = line.Split(' ');
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

                if (subStrings.Length == 1 || subStrings.Length != 3)
                {
                    Console.WriteLine("Error format");
                    return null;
                }
                else
                {
                    i = Convert.ToInt32(subStrings[0]);
                    j = Convert.ToInt32(subStrings[1]);

                    if (i >= j)
                    {
                        Console.WriteLine("Error format");
                        return null;
                    }

                    int k = Convert.ToInt32(subStrings[2]);
                    arr[j][i] = k;
                    arr[i][j] = k;
                }
            }
            return arr;
        }

        static int[] IndexOfMin(int[] array, List<int> que)
        {
            int i = 0;
            int line = array.Last();

            while (i < (array.Length - 1) && array[i] == 0) i++;
            if (i + 1 == array.Length) return new[] { int.MaxValue, line, -1 };

            if (que.Contains(i)) i++;
            int min = int.MaxValue;
            int numb = -1;

            while (i != array.Length - 1)
            {
                if (array[i] <= 0 || que.Contains(i))
                {
                    array[i] = 0;
                    i++;
                    continue;
                }
                else
                {
                    if (array[i] < min)
                    {
                        min = array[i];
                        numb = i;
                    }
                }
                i++;
            }
            return new[] { min, line, numb };
        }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                int totalLength = 0;

                Intracommunicator comm = Communicator.world;

                if (args.Count() != 2)
                {
                    Console.WriteLine("Not enough parametrs. Add input and output files");
                    return;
                }

                string pathIn = "";
                string pathOut = "";
                int[][] parsed = null;
                int[][] tr = new int[comm.Size][];
                List<int> queue = new List<int> { 0 };
                List<int> totalQueue = new List<int> { 0 };
                int[] total = null;

                pathIn = args[0];
                pathOut = args[1];
                if (!File.Exists(pathIn))
                {
                    Console.WriteLine("Nonexistent file. Try again:");
                    return;
                }

                parsed = Pars(pathIn);

                if (parsed == null)
                {
                    Console.WriteLine("NULL");
                    return;
                }

                total = IndexOfMin(parsed[0], totalQueue);

                totalLength += total[0];
                parsed[0][total[2]] = 0;
                parsed[total[2]][0] = 0;
                queue.Add(total[2]);
                totalQueue.Add(total[2]);

                int queueLen = queue.Count;

                while (totalQueue.Count != parsed.Length)
                {
                    int delta = 0;
                    total[0] = int.MaxValue;
                    do
                    {
                        if (comm.Rank == 0)
                        {
                            if (comm.Size > queueLen - delta)
                            {
                                for (int i = 0; i < comm.Size; i++)
                                {
                                    if (i < queueLen - delta) tr[i] = parsed[queue[i + delta]];
                                    else tr[i] = parsed[queue[delta]];
                                }
                            }
                            else
                            {
                                for (int i = 0; i < comm.Size; i++)
                                {
                                    tr[i] = parsed[queue[i + delta]];
                                }
                            }
                        }
                        delta += comm.Size;

                        int[] x = comm.Scatter(tr, 0);

                        int[] min = IndexOfMin(x, totalQueue);

                        if (min[2] == -1)
                        {
                            queue.Remove(min[1]);
                            delta--;
                            queueLen--;
                        }

                        int[][] total1 = comm.Gather(min, 0);
                        
                        if (comm.Rank == 0)
                        {
                            int localMin = total1[0][0];
                            int[] localNum = { total1[0][1], total1[0][2] };
                            foreach (int[] t in total1)
                            {
                                if (localMin > t[0])
                                {
                                    localMin = t[0];
                                    localNum[0] = t[1];
                                    localNum[1] = t[2];
                                }
                            }
                            if (localMin < total[0])
                            {
                                total[0] = localMin;
                                total[1] = localNum[0];
                                total[2] = localNum[1];
                            }
                        }

                        comm.Broadcast(ref total, 0);

                        comm.Barrier();
                    } while ((queueLen - delta) > 0);

                    totalLength = totalLength + total[0];
                    parsed[total[1]][total[2]] = 0;
                    parsed[total[2]][total[1]] = 0;
                    queue.Add(total[2]);
                    totalQueue.Add(total[2]);
                    queueLen = queue.Count;
                    total[0] = int.MaxValue;

                    comm.Barrier();
                }

                if (comm.Rank == 0)
                {
                    string st = Convert.ToString(totalQueue.Count) + System.Environment.NewLine;

                    File.WriteAllText(@pathOut, st + totalLength, Encoding.Unicode);
                }
            }
        }
    }
}
