using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MPI;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using(new MPI.Environment(ref args))
            {
                if (args.Length != 2)
                {
                    Console.WriteLine("You should write down input and output filenames.");
                    return;
                }
                StreamReader inp = new StreamReader(@args[0]);
                string arrayToParse = inp.ReadLine();
                int[][] arrayResult;
                inp.Close();
                int[] array = arrayToParse?.Split(' ').Select(int.Parse).ToArray();
                if(array == null)
                {
                    Console.WriteLine("The array is empty. Nothing to sort.");
                    return;
                }

                Communicator communicator = Communicator.world;

                if(communicator.Size == 1 || array.Length < communicator.Size * communicator.Size)
                {
                    if(communicator.Rank == 0)
                    {
                        Array.Sort(array);
                        StreamWriter outp = new StreamWriter(@args[1]);
                        foreach (int element in array)
                        {
                            outp.Write(element + " ");
                        }
                        outp.WriteLine();
                        outp.Close();
                        Console.WriteLine("Success.");
                    }
                    return;
                }
                arrayResult = Root(array);
                if(communicator.Rank == 0)
                {
                    StreamWriter outp = new StreamWriter(@args[1]);
                    foreach (var list in arrayResult)
                    {
                        foreach(var elem in list)
                        {
                            outp.Write(elem + " ");
                        }
                    }
                    outp.WriteLine();
                    outp.Close();
                    Console.WriteLine("Success.");
                    return;
                }
                return;
            }
        }

        private static void ThirdParty(int n)
        {
            
        }

        private static int[][] Root(int[] array)
        {
            var comm = Communicator.world;
            int n = array.Length;
            int p = comm.Size;
            int k = n / p;
            int t = p / 2;
            int number = comm.Rank;
            var temp = new List<int>();
            for(int i = 0; i < k; i++)
            {
                temp.Add(array[number * k + i]);
            }
            if(number == p - 1 && n % p != 0)
            {
                for(int i = k * p; i < n; i++)
                {
                    temp.Add(array[i]);
                }
            }
            temp.Sort();
            var patterns = new List<int>();
            int w = n / (p * p);
            for(int i = 0; i < p; i++)
            {
                patterns.Add(temp[i * w + 1]);
            }
            var samples = comm.Gather(patterns, 0);
            var pivots = new List<int>();
            if (number == 0)
            {
                List<int> merged = new List<int>();
                Merge(samples.ToList(), ref merged);
                for(int i = 1; i < p; i++)
                {
                    pivots.Add(merged[i * p + t]);
                }
            }
            comm.Broadcast(ref pivots, 0);
            var partitions = new List<int>[p];
            partitions[0] = temp.Where(x => x < pivots[0]).ToList();
            for(int i = 1; i < p - 1; i++)
            {
                partitions[i] = temp.Where(x => x >= pivots[i - 1] && x < pivots[i]).ToList();
            }
            partitions[p - 1] = temp.Where(x => x >= pivots[p - 2]).ToList();
            for(int i = 0; i < p; i++)
            {
                if(i == number)
                {
                    continue;
                }
                comm.Send(partitions[i], i, 1);
            }
            var parts = new List<int>[p];
            for (int i = 0; i < p; i++)
            {
                if (i == number)
                {
                    parts[i] = partitions[i];
                    continue;
                }
                parts[i] = comm.Receive<List<int>>(i, 1);
            }
            List<int> localResult = new List<int>();
            Merge(parts.ToList(), ref localResult);
            var result = comm.Gather(localResult.ToArray(), 0);
            return result;
        }

        static void Merge(List<List<int>> gathered, ref List<int> merged)
        {
            List<int> temp = new List<int>(new int[gathered.Count]);
            bool merging = true;
            while (merging)
            {
                merging = false;
                int min = int.MaxValue;
                int index = -1;

                for(int i = 0; i < temp.Count; i++)
                {
                    if(gathered[i].Count > temp[i] && gathered[i][temp[i]] < min)
                    {
                        min = gathered[i][temp[i]];
                        index = i;
                        merging = true;
                    }
                }

                if(merging)
                {
                    merged.Add(gathered[index][temp[index]]);
                    temp[index]++;
                }
            }
        }
    }
}
