using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace MPITEST
{
    class Program
    {
        static void Main(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                // MAIN PROCESS
                if (comm.Rank == 0)
                {
                    MainProcess main = new MainProcess(comm);

                    // INITIALISATION
                    int numOfProcesses = comm.Size;
                    int[] inputArray = main.ReadArray(args);
                    int arraySrcSize = inputArray.GetLength(0);
                    inputArray = main.fillMaxInt(inputArray, numOfProcesses);
                    int arraySize = inputArray.GetLength(0);
                    int lenOfPart = arraySize / numOfProcesses + ((arraySize % numOfProcesses == 0) ? 0 : 1);

                    main.SortParts( inputArray, lenOfPart);

                    // MAIN - send parts of array at sorting
                    for (int i = 0; i < arraySrcSize; i++)
                    {
                        int sentMes = main.SendTasks(inputArray, lenOfPart, numOfProcesses, i % 2 == 0);
                        inputArray = main.Collect(inputArray, sentMes, lenOfPart);
                    }

                    // OUTPUT - write after collection
                    main.Output(inputArray, arraySrcSize, args);

                    // FREE 
                    main.Free(numOfProcesses);
                }
                else
                {
                    // If not main - whait untill main signal, do your part and send it back
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
