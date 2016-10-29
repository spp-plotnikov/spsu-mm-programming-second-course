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
        static int[][] Pars(string namefile) //парсер файла
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

            for (i = 0; i < arraySize; i ++)
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

            if (i + 1 == array.Length) return new[] { int.MaxValue, -1, line};

            int min = array[i];
            int numb = i;

            i++;

            while (i != array.Length - 1)
            {
                if (array[i] <= 0 || que.Contains(i))
                {
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
            return new[] { min, numb, line};
    }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                int totalLength = 0;

                Intracommunicator comm = Communicator.world;

                if (args.Count() != 2)
                {
                    Console.WriteLine("Not enough parametrs. Add names input and output files");
                    return;
                }

                string pathIn = "";
                string pathOut = "";

                if (Communicator.world.Rank == 0)
                {
                    pathIn = args[0];
                    pathOut = args[1];
                }

                if (!File.Exists(pathIn))
                {
                    Console.WriteLine("Nonexistent file. Try again:");
                    return;
                }

                int[][] parsed = Pars(pathIn); // матрица

                if (parsed == null)
                {
                    Console.WriteLine("NULL");
                    return;
                }

                int[][] tr = new int[comm.Size][]; //для потоков
                List<int> queue = new List<int> { 0 }; //очередь вершин, из которых можно найти минимум
                List<int> totalQueue = new List<int> { 0 }; //итоговая очередь
                int[] total = IndexOfMin(parsed[0], totalQueue); //элемент для стартового сравнения

                totalLength += total[0];
                parsed[0][total[1]] = 0;
                parsed[total[1]][0] = 0;

                queue.Add(total[1]);
                totalQueue.Add(total[1]);
                int queueLen = queue.Count;

                while (totalQueue.Count != parsed.Length)
                {
                    int delta = 0; //для работы с потоками
                    total[0] = int.MaxValue;

                    do
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

                        delta += comm.Size;
                        int[] x = comm.Scatter(tr, 0); //разделение
                        
                        int[] min = IndexOfMin(x, totalQueue);
                        if (min[1] == -1)
                        {
                            queue.Remove(min[2]);
                            delta--;
                            queueLen--;
                        }

                        int[][] total1 = comm.Gather(min, 0); //слияние
                        
                        if (comm.Rank == 0) //обработка подытога 
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

                    } while ((queueLen - delta) > 0);

                    //финальная выборка
                    totalLength = totalLength + total[0];
                    parsed[total[1]][total[2]] = 0;
                    parsed[total[2]][total[1]] = 0;

                    queue.Add(total[1]);
                    totalQueue.Add(total[1]);
                    queueLen = queue.Count;
                    total[0] = int.MaxValue;                   
                }

                if (comm.Rank == 0)
                {
                    string st = "";

                    foreach (var q in totalQueue)
                    {
                        st += q + " ";
                    }

                    st += "\r\n";
                    File.WriteAllText(@pathOut, st + totalLength, Encoding.Unicode);
                 }
            }
        }
    }
}
