using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MPI;



namespace MPIProblem
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            using (new MPI.Environment(ref args))
            {

                var input = new StreamReader(@args[0]);
                var data = input.ReadLine();
                input.Close();

                int[] arr = data?.Split(' ').Select(int.Parse).ToArray();

                if (arr == null)
                {
                    Console.WriteLine("Corrupted input");
                    return;
                }

                if (Communicator.world.Size == 1)
                {
                    Array.Sort(arr);

                    var output = new StreamWriter(@args[1]);
                    foreach (var i in arr)
                    {
                        output.Write(i + " ");
                    }
                    output.WriteLine();
                    output.Close();
                    return;
                }


                Communicator comm = Communicator.world;
                if (comm.Rank == 0)
                {
                    HeadPart(arr, args[1]);
                }
                else
                {
                    ComputePart();
                }
            }
        }


        private static void HeadPart(int[] array, string outputPath)
        {
            /*1. firstPhase: sorting local segment list and sample elements in sorted list.
            2. secondPhase: sampled elements are sent to one machine, say master node, and sorted then broadcasted to all
            working node.
            3. thirdPhase: each working node use these samples to slice sorted list into sub lists and send back the length 
            of these sub lists back to master node so that master node can arrange to receive these sub list in corresponding
            position in next phase.
            4. fourthPhase: receiving these sub lists using temporary buffers . And merge these lists into one final array.
            5. zeroPhase: in order to compare with speed of sorting with single machine, I invented this function.*/
            var len = array.Length;
            var procCount = Communicator.world.Size;

            for (var i = 1; i < procCount; i++)
            {               
                var initalPos = len / procCount + (len % procCount > i ? 1 : 0);
                Communicator.world.Send(initalPos, i, 0);
            }


            var curProcessPart = new List<int>();

            for (var i = 0; i < len; i += procCount)  // firstPhase
            {
                curProcessPart.Add(array[i]);
                for (var j = 1; j + i < len && j < procCount; j++)
                {
                    Communicator.world.Send(array[i + j], j, 1);
                }
            }

            curProcessPart.Sort();

            var tempList = Enumerable.Range(0, curProcessPart.Count/procCount).Select(x => curProcessPart[x*procCount]).ToList();

            for (var i = 1; i < procCount; i++) // secondPhase
            {
                int curNum;
                Communicator.world.Receive(i, 2, out curNum);
                for (var j = 0; j <= curNum; j++)
                {
                    int t;
                    Communicator.world.Receive(i, 3, out t);
                    tempList.Add(t);
                }
            }

            var data = new List<int>();//thirdPhase
            var edges = new List<int>(); 
            for (var i = 0; i < procCount - 1; i++)
            {
                var temp = tempList.Count / procCount + (tempList.Count % procCount > i ? 1 : 0);
                data.Add(tempList[temp]);
            }

            data.Sort();

            for (var i = 0; i < procCount - 1; i++)
            {
                edges.Add(data[i * (data.Count / procCount)]);
            }

            for (var i = 1; i < procCount; i++)
            {
                Communicator.world.Send(edges, i, 2);
            }

            var k = 0;
            var tempResult = new List<List<int>>();
            var tempArr = new List<int>();

            for (var i = 0; i < edges.Count; i++)
            {
                tempArr = new List<int>();
                while (k < curProcessPart.Count && curProcessPart[k] <= edges[i])
                {
                    tempArr.Add(curProcessPart[k]);
                    k++;
                }
                if (i != Communicator.world.Rank)
                {
                    Communicator.world.Send(tempArr, i, 4);
                }
                else if (tempArr.Count != 0)
                {
                    tempResult.Add(tempArr);
                }
            }

            while (k < curProcessPart.Count)
            {
                tempArr.Add(curProcessPart[k]);
                k++;
            }

            if (Communicator.world.Size - 1 != Communicator.world.Rank)
            {
                Communicator.world.Send(tempArr, Communicator.world.Size - 1, 4);
            }
            else if (tempArr.Count != 0)
            {
                tempResult.Add(tempArr);
            }

            for (var i = 0; i < procCount; i++) //fourthPhase
            {
                if (i != Communicator.world.Rank)
                {
                    tempArr = new List<int>();
                    Communicator.world.Receive(i, 4, out tempArr);
                    if (tempArr.Count != 0)
                    {
                        tempResult.Add(tempArr);
                    }
                }
            }
            var result = new List<int>();
            Merge(tempResult, ref result);

            var output = new StreamWriter(outputPath);

            foreach (int i in result)
            {
                output.Write(i + " ");
            }
            for (var i = 1; i < procCount; i++)
            {
                var temp = new List<int>();
                Communicator.world.Receive(i, 5, out temp);
                foreach (var i1 in temp)
                {
                    output.Write(i1 + " ");
                }
            }

            output.WriteLine();
            output.Close();
        }


        private static void ComputePart()
        {
            int curProcArrLen;
            Communicator.world.Receive(0, 0, out curProcArrLen);

            var curProcList = new List<int>();
            for (var i = 0; i < curProcArrLen; ++i)
            {
                int cur;
                Communicator.world.Receive(0, 1, out cur);
                curProcList.Add(cur);
            }

            curProcList.Sort();

            var procCount = Communicator.world.Size;
            var samplesCount = (curProcList.Count - 1) / procCount;

            Communicator.world.Send(samplesCount, 0, 2);

            for (var i = 0; i <= samplesCount; i++)
            {
                Communicator.world.Send(curProcList[i * procCount], 0, 3);
            }

            var edges = new List<int>();
            Communicator.world.Receive(0, 2, out edges);

            var k = 0;
            var tempResult = new List<List<int>>();
            var temp = new List<int>();
            for (var i = 0; i < edges.Count; i++)
            {
                temp = new List<int>();
                while (k < curProcList.Count && curProcList[k] <= edges[i])
                {
                    temp.Add(curProcList[k]);
                    k++;
                }
                if (i != Communicator.world.Rank)
                {
                    Communicator.world.Send(temp, i, 4);
                }
                else if (temp.Count != 0)
                {
                    tempResult.Add(temp);
                }
            }
            while (k < curProcList.Count)
            {
                temp.Add(curProcList[k]);
                k++;
            }
            if (Communicator.world.Size - 1 != Communicator.world.Rank)
            {
                Communicator.world.Send(temp, Communicator.world.Size - 1, 4);
            }
            else if (temp.Count != 0)
            {
                tempResult.Add(temp);
            }

            for (var i = 0; i < procCount; i++)
            {
                if (i != Communicator.world.Rank)
                {
                    var temp1 = new List<int>();
                    Communicator.world.Receive(i, 4, out temp1);
                    if (temp1.Count != 0)
                    {
                        tempResult.Add(temp1);
                    }
                }
            }

            var result = new List<int>();
            Merge(tempResult, ref result);

            Communicator.world.Send(result, 0, 5);
        }


        static void Merge(List<List<int>> tempResult, ref List<int> result)
        {
            var data = new List<int>();
            for (var i = 0; i < tempResult.Count; i++)
            {
                data.Add(0);
            }
            var isFinished = false;
            while (!isFinished)
            {
                isFinished = true;
                var min = int.MaxValue;
                var num = -1;
                for (var i = 0; i < data.Count; i++)
                {
                    if (tempResult[i].Count <= data[i] || tempResult[i][data[i]] >= min) continue;
                    min = tempResult[i][data[i]];
                    num = i;
                    isFinished = false;
                }
                if (!isFinished)
                {
                    result.Add(tempResult[num][data[num]]);
                    data[num]++;
                }
            }
        }


    }
}
