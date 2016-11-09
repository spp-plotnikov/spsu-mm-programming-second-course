using System;
using MPI;

namespace EvenOddSortEasyLife
{
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
}
