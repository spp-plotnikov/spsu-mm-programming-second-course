using System;
using MPI;

namespace EvenOddSortEasyLife
{
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
