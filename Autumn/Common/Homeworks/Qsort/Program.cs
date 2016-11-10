using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using System.Threading;
using MPI;


namespace MpiTaskQsort
{
    // The Qsort algorithm 
    // It has a root process as a task manager 
    // Sends information from root about the subarrays that need to be qsorted
    // Gathering information from processes and determine new tasks
    class Program
    {
        public enum sendRoot { endOfWork = -1, sizeOfArray = 0, arrayItself = 1 }; // declare send operations for root
        public enum sendChild { leftIndex = 0, rightIndex = 1, arrayToPass = 2 };

        // for the root process (num = 0)
        static void Root(ref int[] arr, int arrSize)
        {
            Intracommunicator comm = Communicator.world;
            if (comm.Rank != 0)
            {
                return; // we don't need this (we are in root)
            }

            int numOfProcesses = comm.Size;
            bool[] isUsed = new bool[numOfProcesses];
            for (int i = 1; i < numOfProcesses; i++)
            {
                isUsed[i] = false;
            }

            // store the indices for sorting subarrays in queue
            Queue<Tuple<int, int>> sorting = new Queue<Tuple<int, int>>();
            sorting.Enqueue(new Tuple<int, int>(0, arrSize)); // push the whole array at first

            // make a Queue to catch children's work
            Queue<Tuple<int, int, int>> receiving = new Queue<Tuple<int, int, int>>(); // <numOfprocess, left index, right index>

            bool working = true; // check if we finished
            while (working)
            {
                working = false;
                // send to other processes
                while (sorting.Any()) // there are still some elements left
                {
                    int freePointer = 0; // points to free process
                    for (int i = 1; i < numOfProcesses; i++)
                    {
                        if (isUsed[i] == false)
                        {
                            freePointer = i;
                            isUsed[i] = true;
                            break;
                        }
                    }

                    if (freePointer == 0) // all busy :(
                    {
                        break; // probably we need to read something
                    }

                    Tuple<int, int> curIndices = sorting.Dequeue();
                    int l = curIndices.Item1, r = curIndices.Item2;

                    // prepare a piece of array to send
                    int interSize = r - l;
                    int[] toSend = new int[interSize];
                    for (int i = l; i < r; i++)
                    {
                        toSend[i - l] = arr[i];
                    }

                    comm.Send(interSize, freePointer, (int)sendRoot.sizeOfArray); // send the size of the array
                    comm.Send(toSend, freePointer, (int)sendRoot.arrayItself); // send subarray

                    // wait for receiving 
                    receiving.Enqueue(new Tuple<int, int, int>(freePointer, l, r));
                    working = true;
                }
                // receive form processes
                while (receiving.Any())
                {
                    Tuple<int, int, int> first = receiving.Dequeue();
                    int receiveProcess = first.Item1, l = first.Item2, r = first.Item3;
                    isUsed[receiveProcess] = false; // free the process
                    // receive sorted part
                    int intrL = comm.Receive<int>(receiveProcess, (int)sendChild.leftIndex);
                    int intrR = comm.Receive<int>(receiveProcess, (int)sendChild.rightIndex);
                    int[] receiveSubarr = new int[r - l];
                    comm.Receive(receiveProcess, (int)sendChild.arrayToPass, ref receiveSubarr);

                    for (int i = l; i < r; i++)
                    {
                        arr[i] = receiveSubarr[i - l];
                    }

                    if (intrL + l > l) // need to sort more parts
                    {
                        sorting.Enqueue(new Tuple<int, int>(l, l + intrL + 1));
                    }
                    if (l + intrR < r)
                    {
                        sorting.Enqueue(new Tuple<int, int>(l + intrR, r));
                    }
                    working = true;
                }
            }
            for (int i = 1; i < numOfProcesses; i++)
            {
                comm.Send((int)sendRoot.endOfWork, i, 0);
            }
            return;
        }

        // for all other processes
        static void Child()
        {
            Intracommunicator comm = Communicator.world;
            if (comm.Rank == 0)
            {
                return; // we don't need this (we are in child)
            }
            while (true)
            {
                int arrSize = comm.Receive<int>(0, (int)sendRoot.sizeOfArray);
                if (arrSize == -1)
                {
                    return; // the end
                }
                int[] newArr = new int[arrSize];
                comm.Receive(0, (int)sendRoot.arrayItself, ref newArr);
                int idx1 = 0, idx2 = arrSize - 1;
                int checkEl = newArr[arrSize / 2];
                while (idx1 <= idx2)
                {
                    while (newArr[idx1] < checkEl)
                        idx1++;
                    while (newArr[idx2] > checkEl)
                        idx2--;

                    if (idx1 <= idx2)
                    {
                        int temp = newArr[idx1];
                        newArr[idx1++] = newArr[idx2];
                        newArr[idx2--] = temp;
                    }
                }
                comm.Send(idx2, 0, (int)sendChild.leftIndex);
                comm.Send(idx1, 0, (int)sendChild.rightIndex);
                comm.Send(newArr, 0, (int)sendChild.arrayToPass);
            }
        }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                if (args.Count() != 2)
                {
                    throw new System.InvalidOperationException("Too few parametrs! Lacks filenames!");
                }

                if (Communicator.world.Rank == 0)
                {
                    // read the files
                    string inputFileName = args[0], outputFileName = args[1];
                    System.IO.StreamReader inputFile = new System.IO.StreamReader(@inputFileName);
                    string rawLine = inputFile.ReadLine();
                    String[] splitInitLine = rawLine.Split(' ');
                    inputFile.Close();

                    // make a comfortable representation 
                    int sizeOfArr = splitInitLine.Count();
                    int[] initArr = new int[sizeOfArr];
                    for (int i = 0; i < sizeOfArr; i++)
                    {
                        initArr[i] = Int32.Parse(splitInitLine[i]);
                    }

                    // sort
                    Root(ref initArr, sizeOfArr);

                    // write into output file
                    System.IO.StreamWriter outputFile = new System.IO.StreamWriter(@outputFileName);
                    for (int i = 0; i < sizeOfArr; i++)
                    {
                        outputFile.Write(initArr[i] + " ");
                    }
                    outputFile.Close();

                    return;
                }
                else
                {
                    Child();
                    return;
                }
            }
        }
    }
}
