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
                try
                {
                    int worldSize = world.Size;

                    int[] array = ArrayActions.Reader(args[0]);
                    int[] recArray, tempRecArray;
                    int[][] sendArray = new int[worldSize][];

                    int blockSize = array.Length / (worldSize);
                    bool diffBlockSize;
                    if (array.Length % (worldSize) != 0)
                        diffBlockSize = true;
                    else
                        diffBlockSize = false;

                    if (world.Rank == 0)
                    {
                        if (array.Length < worldSize)
                        {
                            Console.WriteLine("Invalid number of process");
                            return;
                        }

                        for (int i = 0; i < worldSize - 1; i++)
                            sendArray[i] = new int[blockSize];

                        if (diffBlockSize)
                            sendArray[worldSize - 1] = new int[blockSize + array.Length % worldSize];
                        else
                            sendArray[worldSize - 1] = new int[blockSize];

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

                        for (int i = 1; i < worldSize; i++)
                        {
                            world.Send(new List<int>(sendArray[i]), i, 0);
                        }

                        recArray = sendArray[0];
                    }
                    else
                    {
                        recArray = world.Receive<List<int>>(0, 0).ToArray();
                    }

                    world.Barrier();

                    //if (worldSize == 1)
                    //    SortArrayPart(ref array);

                    for (int n = 0; n < worldSize; n++)
                    {
                        if (n % 2 == 0)
                        {
                            for (int j = 0; j < worldSize - 1; j += 2)
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
                                    if (diffBlockSize && world.Rank == worldSize - 1)
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
                                        recArray = ArrayActions.GetArrayPart(mergedArray, blockSize, 2 * blockSize);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 1; j < worldSize - 1; j += 2)
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
                                    if (diffBlockSize && world.Rank == worldSize - 1)
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
                                        recArray = ArrayActions.GetArrayPart(mergedArray, blockSize, 2 * blockSize);
                                    }
                                }
                            }
                        }
                    }
                    world.Barrier();
                    for (int j = 0; j < worldSize; j++)
                    {
                        world.Barrier();
                        if (j == world.Rank)
                            ArrayActions.Writer(recArray, args[1]);
                    }
                    world.Barrier();
                }
                catch (Exception)
                {
                    if (world.Rank == 0)
                    {
                        Console.WriteLine("Invalid Data");
                    }
                }
            }
        }
    }
}
