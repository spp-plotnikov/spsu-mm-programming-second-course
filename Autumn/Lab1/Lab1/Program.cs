using System;
using System.Collections.Generic;
using MPI;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                var world = Communicator.world;

                int worldSize = world.Size - 1;
                if (args.Length == 2)
                {
                    if (args[0].Substring(args[0].Length - 4) == ".txt")
                    {
                        int[] array = ArrayActions.Reader(args[0]);
                        int[] recArray, tempRecArray;
                        int[][] sendArray = new int[worldSize][];
                        int[] newArray = new int[array.Length];
                        if (worldSize != 0)
                        {
                            int blockSize = array.Length / (worldSize);
                            bool diffBlockSize = array.Length % (worldSize) != 0;

                            if (world.Rank == 0)
                            {
                                if (array.Length < worldSize)
                                {
                                    Console.WriteLine("Invalid number of process");
                                    return;
                                }

                                if (args[1].Substring(args[1].Length - 4) != ".txt")
                                {
                                    Console.WriteLine("Invalid name of output file");
                                    return;
                                }

                                for (int i = 0; i < worldSize - 1; i++)
                                {
                                    sendArray[i] = new int[blockSize];
                                }

                                if (diffBlockSize)
                                {
                                    sendArray[worldSize - 1] = new int[blockSize + array.Length % worldSize];
                                }
                                else
                                {
                                    sendArray[worldSize - 1] = new int[blockSize];
                                }

                                for (int i = 0; i < worldSize - 1; i++)
                                {
                                    for (int j = 0; j < blockSize; j++)
                                        sendArray[i][j] = array[i * blockSize + j];
                                }

                                if (diffBlockSize)
                                {
                                    for (int j = 0; j < blockSize + array.Length % worldSize; j++)
                                        sendArray[worldSize - 1][j] = array[blockSize * (worldSize - 1) + j];
                                }
                                else
                                {
                                    for (int j = 0; j < blockSize; j++)
                                        sendArray[worldSize - 1][j] = array[(worldSize - 1) * blockSize + j];
                                }

                                for (int i = 0; i < worldSize; i++)
                                {
                                    world.Send(new List<int>(sendArray[i]), i + 1, 0);
                                }
                            }
                            else
                            {
                                recArray = world.Receive<List<int>>(0, 0).ToArray();
                                world.Barrier();
                                for (int n = 1; n < worldSize + 1; n++)
                                {
                                    if (n % 2 == 0)
                                    {
                                        for (int j = 1; j < worldSize; j += 2)
                                        {
                                            if (world.Rank == j)
                                            {
                                                world.Send(new List<int>(recArray), j + 1, j);

                                                tempRecArray = world.Receive<List<int>>(j + 1, j + 1).ToArray();
                                                int[] mergedArray = ArrayActions.MergeArrays(recArray, tempRecArray);
                                                ArrayActions.SortArrayPart(ref mergedArray);
                                                recArray = ArrayActions.GetArrayPart(mergedArray, 0, blockSize);

                                            }
                                            else if (world.Rank == j + 1)
                                            {
                                                world.Send(new List<int>(recArray), j, j + 1);

                                                tempRecArray = world.Receive<List<int>>(j, j).ToArray();
                                                if (diffBlockSize && world.Rank == worldSize)
                                                {
                                                    int[] mergedArray = ArrayActions.MergeArrays(recArray, tempRecArray);
                                                    ArrayActions.SortArrayPart(ref mergedArray);
                                                    recArray = ArrayActions.GetArrayPart(mergedArray, blockSize,
                                                        2 * blockSize + array.Length % worldSize);
                                                }
                                                else
                                                {
                                                    int[] mergedArray = ArrayActions.MergeArrays(recArray, tempRecArray);
                                                    ArrayActions.SortArrayPart(ref mergedArray);
                                                    recArray = ArrayActions.GetArrayPart(mergedArray, blockSize,
                                                        2 * blockSize);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int j = 2; j < worldSize; j += 2)
                                        {
                                            if (world.Rank == j)
                                            {
                                                world.Send(new List<int>(recArray), j + 1, j);

                                                tempRecArray = world.Receive<List<int>>(j + 1, j + 1).ToArray();
                                                int[] mergedArray = ArrayActions.MergeArrays(recArray, tempRecArray);
                                                ArrayActions.SortArrayPart(ref mergedArray);
                                                recArray = ArrayActions.GetArrayPart(mergedArray, 0, blockSize);

                                            }
                                            else if (world.Rank == j + 1)
                                            {
                                                world.Send(new List<int>(recArray), j, j + 1);
                                                tempRecArray = world.Receive<List<int>>(j, j).ToArray();
                                                if (diffBlockSize && world.Rank == worldSize)
                                                {
                                                    int[] mergedArray = ArrayActions.MergeArrays(recArray, tempRecArray);
                                                    ArrayActions.SortArrayPart(ref mergedArray);
                                                    recArray = ArrayActions.GetArrayPart(mergedArray, blockSize,
                                                        2 * blockSize + array.Length % worldSize);
                                                }
                                                else
                                                {
                                                    int[] mergedArray = ArrayActions.MergeArrays(recArray, tempRecArray);
                                                    ArrayActions.SortArrayPart(ref mergedArray);
                                                    recArray = ArrayActions.GetArrayPart(mergedArray, blockSize,
                                                        2 * blockSize);
                                                }
                                            }
                                        }
                                    }
                                }
                                for (int j = 1; j < worldSize + 1; j++)
                                {
                                    if (j == world.Rank)
                                    {
                                        world.Send(new List<int>(recArray), 0, j + worldSize);
                                    }
                                }
                            }

                            if (world.Rank == 0)
                            {

                                int count = 0;
                                world.Barrier();
                                for (int i = 1; i < worldSize + 1; i++)
                                {
                                    recArray = world.Receive<List<int>>(i, i + worldSize).ToArray();
                                    for (int k = 0; k < recArray.Length; k++)
                                    {
                                        newArray[count] = recArray[k];
                                        count++;
                                    }
                                }
                                ArrayActions.Writer(newArray, args[1]);
                            }
                            world.Barrier();
                        }
                        else
                        {
                            if (world.Rank == 0)
                            {
                                ArrayActions.SortArrayPart(ref array);
                                ArrayActions.Writer(array, args[1]);
                            }
                        }
                    }
                    else
                    {
                        if (world.Rank == 0)
                        {
                            Console.WriteLine("Invalid name of input file");
                            return;
                        }
                    }
                }
                else
                {
                    if (world.Rank == 0)
                    {
                        Console.WriteLine("Invalid number of inputs");
                        return;
                    }
                }
            }
        }
    }
}

