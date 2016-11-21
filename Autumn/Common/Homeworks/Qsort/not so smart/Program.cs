using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using System.Threading;
using MPI;

namespace Qsort
{
    class Program
    {
        // declare send operations for root
        public enum SendRoot 
        { 
            EndOfWork = -1,
            SizeOfArray = 0, 
            ArrayItself = 1 
        }

        // sequential qsort
        public static void PlQsort(ref int[] arr, int l, int r)
        {
            int i = l, j = r;
            int pivot = arr[(i + j) / 2];
            while (i <= j)
            {
                while (arr[i] < pivot)
                {
                    i++;
                }

                while (arr[j] > pivot)
                {
                    j--;
                }

                if(i <= j)
                {
                    // swap
                    int tmp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = tmp;
                    i++;
                    j--;
                }
            }

            if(l < j)
            {
                PlQsort(ref arr, l, j);
            }

            if(i < r)
            {
                PlQsort(ref arr, i, r);
            }
        }

        // the Root process
        static void Root(ref int[] arr, int arrSize)
        {
            Intracommunicator comm = Communicator.world;
            if(comm.Rank != 0)
            {
                return; // we don't need this (we are in root)
            }

            int numOfProcesses = comm.Size - 1;

            // if there are too many parts to divide to
            if(numOfProcesses > arrSize)
            {
                numOfProcesses = arrSize;
            }

            int numPerOne = arrSize / numOfProcesses;
            
            // we will hold the last processor seperately
            // here are all except the last
            for (int i = 0; i < numOfProcesses - 1; i++)
            {
                int[] toSend = new int[numPerOne];
                int tmp = i * numPerOne;
                for (int j = tmp; j < tmp + numPerOne; j++)
                {
                    toSend[j - tmp] = arr[j];
                }

                comm.Send(numPerOne, (i + 1), (int)SendRoot.SizeOfArray); // send the size of the array
                comm.Send(toSend, (i + 1), (int)SendRoot.ArrayItself); // send subarray
            }

            // the last processor case
            int lastLen = numPerOne + arrSize % numOfProcesses;
            int[] lastSend = new int[lastLen];
            for (int i = (numOfProcesses - 1) * numPerOne; i < arrSize; i++)
            {
                lastSend[i - (numOfProcesses - 1) * numPerOne] = arr[i];
            }

            comm.Send(lastLen, numOfProcesses, (int)SendRoot.SizeOfArray); // send the size of the array
            comm.Send(lastSend, numOfProcesses, (int)SendRoot.ArrayItself); // send subarray

            // if there are too many processes abort exceeding
            for (int i = numOfProcesses + 1; i < comm.Size; i++)
            {
                comm.Send(-1, i, (int)SendRoot.SizeOfArray); // send the size of the array
            }

            // we will store results in a separate copy
            int[] newCopy = new int[arrSize];

            // now receive all the parts 
            for (int i = 0; i < numOfProcesses; i++)
            {
                int subarrSize = comm.Receive<int>(i + 1, (int)SendRoot.SizeOfArray);
                int[] newArr = new int[subarrSize];
                comm.Receive(i + 1, (int)SendRoot.ArrayItself, ref newArr);
                for (int j = 0; j < subarrSize; j++)
                {
                    newCopy[j + i * numPerOne] = newArr[j];
                }
            }

            // merge sorted parts
            int idx = 0;
            int[] partsPtr = new int[numOfProcesses];

            for (int i = 0; i < numOfProcesses; i++)
            {
                partsPtr[i] = i * numPerOne; // shows the first element of subarray
            }

            while (idx < arrSize)
            {
                int tmpIdx = 0;
                int minElem = Int32.MaxValue;

                // looking through new min
                for (int i = 0; i < numOfProcesses; i++)
                {
                     // denote that we went through the whole subarray
                     if(partsPtr[i] != -1)
                     {
                         if(newCopy[partsPtr[i]] < minElem)
                         {
                             minElem = newCopy[partsPtr[i]];
                             tmpIdx = i;
                         }
                     }
                }

                arr[idx] = minElem;
                idx++;
                
                // move index in subarray

                // special case last pointer
                if(tmpIdx == (numOfProcesses - 1))
                {
                    if(partsPtr[tmpIdx] == arrSize - 1)
                    {
                        partsPtr[tmpIdx] = -1; // we have finished with this part
                    }
                    else
                    {
                        partsPtr[tmpIdx]++;
                    }
                }
                else
                {
                    if(partsPtr[tmpIdx] == ((tmpIdx  + 1) * numPerOne - 1))
                    {
                        partsPtr[tmpIdx] = -1; // we have finished with this part
                    }
                    else
                    {
                        partsPtr[tmpIdx]++;
                    }
                }
            }

            return;
        }

        // Executes by all except the root process 
        static void Child()
        {
            Intracommunicator comm = Communicator.world;
            if(comm.Rank == 0)
            {
                return; // we don't need this (we are in child)
            }

            int arrSize = comm.Receive<int>(0, (int)SendRoot.SizeOfArray);
            if(arrSize == -1)
            {
                return; // we don't need this process
            }

            int[] newArr = new int[arrSize];
            comm.Receive(0, (int)SendRoot.ArrayItself, ref newArr);
            PlQsort(ref newArr, 0, arrSize - 1);
            comm.Send(arrSize, 0, (int)SendRoot.SizeOfArray);
            comm.Send(newArr, 0, (int)SendRoot.ArrayItself);
            return;
        }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                if(args.Count() != 2)
                {
                    throw new System.InvalidOperationException("Too few parameters! Lacks filenames!");
                }

                if(Communicator.world.Rank == 0)
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
