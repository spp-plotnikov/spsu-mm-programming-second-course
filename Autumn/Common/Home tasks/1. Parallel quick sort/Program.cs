using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics;
using System.Threading;
using MPI;

namespace parallelQsort
{
    class Program
    {
        static void rootProccess(ref int[] arr, int size)
        {
            Intracommunicator comm = Communicator.world;
            if (comm.Rank == 0)
            {
                int numProcess = comm.Size;
                bool[] isUse = new bool[numProcess];
                for (int i = 1; i < numProcess; i++)
                {
                    isUse[i] = false;
                }

                Tuple<int, int> indexPair = new Tuple<int, int>(0, size);
                Queue<Tuple <int, int> > sortingQueue = new Queue<Tuple <int, int> >();
                sortingQueue.Enqueue(indexPair);
                Queue<Tuple <int, int, int> > catchingQueue = new Queue<Tuple <int, int, int> >();
                bool isWorking = true;
                while (isWorking)
                {
                    isWorking = false;
                    while (sortingQueue.Count != 0) // sending 
                    {
                        isWorking = true;
                        int freeProccess = 0;
                        
                        for (int i = 1; i < numProcess; i++) // find first free proccess
                        {
                            if (isUse[i] == false)
                            {
                                isUse[i] = true;
                                freeProccess = i;
                                break;
                            }
                        }
                        if (freeProccess == 0) // if no 1 free process then try to recieve something
                        {
                            break;
                        }

                        Tuple <int, int> pair = sortingQueue.Dequeue();
                        int left = pair.Item1;
                        int right = pair.Item2;
                                               
                        int[] sendArr = new int[right - left];
                        for (int i = left; i < right; i++)
                        {
                            sendArr[i - left] = arr[i];
                        }
                        comm.Send(right - left, freeProccess, 0); // send size of array
                        comm.Send(sendArr, freeProccess, 1);      // send part of array
                        Tuple<int, int, int> addedAtQueue = new Tuple<int, int, int>(freeProccess, left, right);
                        catchingQueue.Enqueue(addedAtQueue);     // add to recieving queue
                    }

                    while (catchingQueue.Count != 0)             // recieving   
                    {
                        isWorking = true;
                        Tuple<int, int, int> front = catchingQueue.Dequeue();
                        int numProccess = front.Item1;
                        int left = front.Item2;
                        int right = front.Item3;
                        
                        isUse[numProccess] = false;
                        int newL = comm.Receive<int>(numProccess, 0);
                        int newR = comm.Receive<int>(numProccess, 1);
                        int[] receiveArr = new int[right - left];
                        comm.Receive(numProccess, 2, ref receiveArr);

                        for (int i = left; i < right; i++)
                        {
                            arr[i] = receiveArr[i - left];
                        }                  
                        
                        if (newL + left > left)
                        {
                            Tuple<int, int> newElem = new Tuple<int, int>(left, left + newL + 1);
                            sortingQueue.Enqueue(newElem);
                        }
                        if (left + newR < right)
                        {
                            Tuple<int, int> newElem = new Tuple<int, int>(left + newR, right);
                            sortingQueue.Enqueue(newElem);
                        }                                             
                    }
                }
                
                for (int i = 1; i < comm.Size; i++) // end of program
                {
                    comm.Send(-1, i, 0);
                }
                return;              
            }
            else
            {
                return;
            }
        }

        static void childProcess()
        {
            Intracommunicator comm = Communicator.world;
            if (comm.Rank != 0)
            {
                while (true)
                {
                    int size = comm.Receive<int>(0, 0);
                    if (size == -1) // end of program
                    {
                        return;
                    }

                    int[] arr = new int[size];
                    comm.Receive(0, 1, ref arr);
                    
                    int i = 0;
                    int j = size - 1;
                    int refElem = arr[size / 2];  

                    while (i <= j)
                    {
                        while (arr[i] < refElem) i++;
                        while (arr[j] > refElem) j--;

                        if (i <= j)
                        {
                            int tmp = arr[i];
                            arr[i] = arr[j];
                            arr[j] = tmp;
                            i++;
                            j--;
                        }
                    }
                    
                    comm.Send(j, 0, 0);
                    comm.Send(i, 0, 1);
                    comm.Send(arr, 0, 2);
                }
            }
            else
            {
                return;
            }
        }

        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                if (args.Count() != 2)
                {
                    Console.WriteLine("Invalid number of input parameters. 2 filenames expected.");
                    return;
                }

                if (Communicator.world.Rank == 0)
                {

                    string fileNameIn = args[0];
                    string fileNameOut = args[1];

                    System.IO.StreamReader fileIn = new System.IO.StreamReader(@fileNameIn);
                    string line = fileIn.ReadLine();
                    fileIn.Close();
                    String[] st = line.Split(' ');
                    
                    int size = st.Count();
                    int[] arr = new int[size];

                    for (int i = 0; i < size; i++)
                    {
                        arr[i] = Int32.Parse(st[i]);
                    }

                    rootProccess(ref arr, size);
                    
                    System.IO.StreamWriter fileOut = new System.IO.StreamWriter(@fileNameOut);
                    for (int i = 0; i < size; i++)
                    {
                        fileOut.Write(arr[i] + " ");
                    }
                    fileOut.WriteLine();
                    fileOut.Close();

                    return;
                }
                else
                {
                    childProcess();
                    return;
                }
            }
        }
    }
}
