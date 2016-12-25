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
                    Console.ReadKey();
                    return;
                }
                StreamReader inp = new StreamReader(@args[0]);
                string arrayToParse = inp.ReadLine();
                int[] arrayResult;
                inp.Close();
                int[] array = arrayToParse?.Split(' ').Select(int.Parse).ToArray();
                if(array == null)
                {
                    Console.WriteLine("The array is empty. Nothing to sort.");
                    return;
                }

                Communicator communicator = Communicator.world;

                if(communicator.Size == 1)
                {
                    Array.Sort(array);
                    StreamWriter outp = new StreamWriter(@args[1]);
                    foreach(int element in array)
                    {
                        outp.Write(element + " ");
                    }
                    outp.WriteLine();
                    outp.Close();
                    Console.WriteLine("Success.");
                    return;
                }
                if(communicator.Rank == 0)
                {
                    arrayResult = Root(array);
                    StreamWriter outp = new StreamWriter(@args[1]);
                    foreach (int element in arrayResult)
                    {
                        outp.Write(element + " ");
                    }
                    outp.WriteLine();
                    outp.Close();
                    Console.WriteLine("Success.");
                    return;
                }
                else
                {
                    ThirdParty();
                }
            }
        }
        private static int[] Root(int[] array)
        {
            Communicator communicator = Communicator.world;
            int length = array.Length;
            int p = communicator.Size;
            for(var i = 1; i < p; i++)
            {
                int start = length / p;
                start += ((length % p > i) ? 1 : 0);
                communicator.Send(start, i, 0);
            }

            List<int> current = new List<int>();

            for(int i = 0; i < length; i += p)
            {
                current.Add(array[i]);
                for(int j = 1; j + i < length && j < p; j++)
                {
                    communicator.Send(array[i + j], j, 1);
                }
            }
            current.Sort();
            List<int> samples = Enumerable.Range(0, current.Count / p).Select(x => current[x * p]).ToList();

            for(int i = 1; i < p; i++)
            {
                int elem;
                communicator.Receive(i, 2, out elem);
                for (int j = 0; j <= elem; j++)
                {
                    int temp;
                    communicator.Receive(i, 3, out temp);
                    samples.Add(temp);
                }
            }

            List<int> data = new List<int>();
            List<int> pivots = new List<int>();
            for(int i = 0; i < p - 1; i++)
            {
                int number = samples.Count / p;
                number += ((samples.Count % p > i) ? 1 : 0);
                data.Add(samples[number]);
            }

            data.Sort();

            for(int i = 0; i < p - 1; i++)
            {
                pivots.Add(data[i * (data.Count / p)]);
            }

            for(int i = 1; i < p; i++)
            {
                communicator.Send(pivots, i, 2);
            }

            int k = 0;
            List<List<int>> gathered = new List<List<int>>();
            List<int> gatheringArray = new List<int>();

            for(int i = 0; i < pivots.Count; i++)
            {
                gatheringArray = new List<int>();
                while(k < current.Count && current[k] <= pivots[i])
                {
                    gatheringArray.Add(current[k]);
                    k++;
                }
                if(i != communicator.Rank)
                {
                    communicator.Send(gatheringArray, i, 4);
                }
                else if(gatheringArray.Count != 0)
                {
                    gathered.Add(gatheringArray);
                }
            }

            while(k < current.Count)
            {
                gatheringArray.Add(current[k]);
                k++;
            }

            if(communicator.Size - 1 != communicator.Rank)
            {
                communicator.Send(gatheringArray, communicator.Size - 1, 4);
            }
            else if(gatheringArray.Count != 0)
            {
                gathered.Add(gatheringArray);
            }

            for(int i = 0; i < p; i++)
            {
                if(i != communicator.Rank)
                {
                    gatheringArray = new List<int>();
                    communicator.Receive(i, 4, out gatheringArray);
                    if (gatheringArray.Count != 0)
                    {
                        gathered.Add(gatheringArray);
                    }
                }
            }
            List<int> merged = new List<int>();
            Merge(gathered, ref merged);
            List<int> result = new List<int>();
            foreach(int i in merged)
            {
                result.Add(i);
            }
            for(int i = 1; i < p; i++)
            {
                List<int> temp = new List<int>();
                communicator.Receive(i, 5, out temp);
                foreach(int elem in temp)
                {
                    result.Add(elem);
                }
            }
            Console.WriteLine("Success.");
            return result.ToArray();
        }

        private static void ThirdParty()
        {
            Communicator communicator = Communicator.world;
            int length;
            communicator.Receive(0, 0, out length);

            List<int> localList = new List<int>();
            for(int i = 0; i < length; i++)
            {
                int elem;
                communicator.Receive(0, 1, out elem);
                localList.Add(elem);
            }

            localList.Sort();

            int p = communicator.Size;
            int samples = (localList.Count - 1) / p;

            communicator.Send(samples, 0, 2);

            for(var i = 0; i <= samples; i++)
            {
                communicator.Send(localList[i * p], 0, 3);
            }

            var pivots = new List<int>();
            communicator.Receive(0, 2, out pivots);

            int j = 0;
            var gatheredList = new List<List<int>>();
            List<int> temp = new List<int>();
            for(var i = 0; i < pivots.Count; i++)
            {
                temp = new List<int>();
                while (j < localList.Count && localList[j] <= pivots[i])
                {
                    temp.Add(localList[j]);
                    j++;
                }
                if (i != communicator.Rank)
                {
                    communicator.Send(temp, i, 4);
                }
                else if (temp.Count != 0)
                {
                    gatheredList.Add(temp);
                }
            }
            while(j < localList.Count)
            {
                temp.Add(localList[j]);
                j++;
            }
            if(communicator.Size - 1 != communicator.Rank)
            {
                communicator.Send(temp, communicator.Size - 1, 4);
            }
            else if(temp.Count != 0)
            {
                gatheredList.Add(temp);
            }
            for(var i = 0; i < p; i++)
            {
                if(i != communicator.Rank)
                {
                    List<int> giveAway = new List<int>();
                    communicator.Receive(i, 4, out giveAway);
                    if (giveAway.Count != 0)
                    {
                        gatheredList.Add(giveAway);
                    }
                }
            }

            List<int> merged = new List<int>();
            Merge(gatheredList, ref merged);
            communicator.Send(merged, 0, 5);
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
