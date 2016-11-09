using System;
using MPI;

namespace EvenOddSortEasyLife
{
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
}
