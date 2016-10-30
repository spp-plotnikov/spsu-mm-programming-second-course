using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace EvenOddSortEasyLife
{
    [Serializable]
    public class Task
    {
        public int[] arr1;
        public int[] arr2;
        public int firstIndex;

        public Task(int[] ar1, int[] ar2, int f)
        {
            arr1 = ar1;
            arr2 = ar2;
            firstIndex = f;
        }
    }

    [Serializable]
    public class Answer
    {
        public int[] arr;
        public int firstIndex;

        public Answer(int[] ar, int f)
        {
            arr = ar;
            firstIndex = f;
        }
    }

    public class MainProcess
    {
        Intracommunicator comm;
        
        public MainProcess(Intracommunicator com)
        {
            comm = com;
        }

        public void SortParts(ref int[] inputArray, int lenOfPart) // sorts the parts of array in the beginning
        {
            for (int i = 0; i < inputArray.GetLength(0); i += lenOfPart)
            {
                int[] part = SubArray(inputArray, i, lenOfPart);
                Array.Sort(part);
                Array.Copy(part, 0, inputArray, i, lenOfPart);
            }
        }

        public int[] addMaxInt(int[] inputArray, int numOfParts) // adds MaxInt to the end of array for nice sharing between processes
        {
            int arraySize = inputArray.GetLength(0);
            int resLength = numOfParts * (arraySize / numOfParts + ((arraySize % numOfParts == 0) ? 0 : 1));
            int numOfMaxInt = resLength - arraySize;
            int[] res = new int[resLength];
            Array.Copy(inputArray, 0, res, 0, arraySize);
            for (int i = 0; i < numOfMaxInt; i++)
            {
                res[arraySize + i] = Int32.MaxValue;
            }
            return res;
        }

        public int[] ReadArray(string[] args) // Reads the source array
        {
            if (args.GetLength(0) != 2)
            {
                Console.WriteLine("Invalid input parameters");
                System.Environment.Exit(400);
            }
            string inputFileName = args[0];
            System.IO.StreamReader inputFile = new System.IO.StreamReader(inputFileName);
            string line = inputFile.ReadLine();
            string[] nums = line.Split(' ');
            int size = nums.GetLength(0);
            int[] a = new int[size];
            for (int i = 0; i < size; i++)
            {
                a[i] = Convert.ToInt32(nums[i]);
            }
            inputFile.Close();
            return a;
        }

        public void WriteAnswer(int[] answer, int arraySrcSize, string[] args) // writes the answer
        {
            string outputFileName = args[1];
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter(outputFileName);
            for (int i = 0; i < arraySrcSize; i++)
            {
                outputFile.Write("{0} ", answer[i]);
            }
            outputFile.Close();
        }

        public int[] SubArray(int[] data, int index, int length) // get subarray
        {
            int[] result = new int[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public int SendIterTasks(int[] inputArray, int lenOfPart, int numOfProcesses, bool isEven) // sends tasks to processes and returns num of sent tasks
        {
            int sentTasks = 0;
            int shift = (isEven) ? 0 : 1;

            for (int i = 1 + shift; i < numOfProcesses; i += 2)
            {
                Task task = new Task(SubArray(inputArray, (i - 1) * lenOfPart, lenOfPart), SubArray(inputArray, i * lenOfPart, lenOfPart), (i - 1) * lenOfPart);
                comm.Send<Task>(task, i, 0);
                sentTasks++;
            }
            return sentTasks;
        }

        public int[] ReceiveNChange(int[] inputArray, int numOfMessages, int lenOfPart) // receive n answers and change the array
        {
            for (int i = 0; i < numOfMessages; i++)
            {
                Answer ans = comm.Receive<Answer>(Communicator.anySource, 0);
                Array.Copy(ans.arr, 0, inputArray, ans.firstIndex, lenOfPart * 2);
            }
            return inputArray;
        }

        public void FreeGuys(int numOfProcesses) // signals the end of work
        {
            Task emptyTask = new Task(new int[0], new int[0], -1);
            for (int i = 1; i < numOfProcesses; i++)
            {
                comm.Send<Task>(emptyTask, i, 0);
            }
        }
    }

    class WorkProcess
    {
        Intracommunicator comm;

        public WorkProcess(Intracommunicator com)
        {
            comm = com;
        }

        int[] MergeSort(int[] arr1, int[] arr2) // just mergesort
        {
            Func<int, int, bool> isFirstMin = (a, b) => (a < b) ? true : false;
            int length1 = arr1.GetLength(0);
            int length2 = arr2.GetLength(0);
            int[] res = new int[length1 + length2];
            int i = 0, j = 0, k = 0;
            bool isFirstminIndex;
            for (k = 0; k < length1 + length2; k++)
            {
                if (i < length1 && j < length2)
                {
                    isFirstminIndex = isFirstMin(arr1[i], arr2[j]);
                }
                else
                {
                    if (i >= length1) { isFirstminIndex = false; }
                    else { isFirstminIndex = true; }                 
                }

                if (isFirstminIndex)
                {
                    res[k] = arr1[i];
                    i++;
                }
                else
                {
                    res[k] = arr2[j];
                    j++;
                }
            }
            return res;
        }

        public Task ReceiveTaskFromMain() // receives task
        {
            return comm.Receive<Task>(0, 0);
        }

        public void SendAnswerToMain(Answer ans) // sends solved task
        {
            comm.Send<Answer>(ans, 0, 0);
        }

        public int[] SolveTask(Task task) // solves task
        {
            return MergeSort(task.arr1, task.arr2);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                if (comm.Rank == 0)
                {
                    MainProcess main = new MainProcess(comm);
                    // Just inits before working
                    int numOfProcesses = comm.Size;
                    int[] inputArray = main.ReadArray(args);
                    int arraySrcSize = inputArray.GetLength(0);
                    inputArray = main.addMaxInt(inputArray, numOfProcesses);
                    int arraySize = inputArray.GetLength(0);
                    int lenOfPart = arraySize / numOfProcesses + ((arraySize % numOfProcesses == 0) ? 0 : 1);
                    main.SortParts(ref inputArray, lenOfPart);

                    // the main part is here
                    for (int i = 0; i < arraySrcSize; i++)
                    {
                        int sentMes = main.SendIterTasks(inputArray, lenOfPart, numOfProcesses, i % 2 == 0);
                        inputArray = main.ReceiveNChange(inputArray, sentMes, lenOfPart);
                    }

                    // writing answer
                    main.WriteAnswer(inputArray, arraySrcSize, args);

                    // free other processes
                    main.FreeGuys(numOfProcesses);
                }
                else
                {
                    WorkProcess work = new WorkProcess(comm);
                    while (true)
                    {
                        Task task = work.ReceiveTaskFromMain();
                        if (task.firstIndex == -1) { break; }
                        int[] ans = work.SolveTask(task);
                        work.SendAnswerToMain(new Answer(ans, task.firstIndex));
                    }
               }
            }
        }
    }
}
