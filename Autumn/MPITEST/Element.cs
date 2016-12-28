using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPITEST
{
    class WorkProcess
    {
        Intracommunicator comm;

        public WorkProcess(Intracommunicator com)
        {
            comm = com;
        }

        int[] MergeSort(int[] arr1, int[] arr2) 
        {
            int len1 = arr1.GetLength(0);
            int len2 = arr2.GetLength(0);

            int[] res = new int[len1 + len2];
            int i = 0, j = 0;
            bool isFirstMin;

            for (int k = 0; k < len1 + len2; k++)
            {
                if (i < len1 && j < len2)
                    isFirstMin = (arr1[i] < arr2[j]) ? true : false;

                else // out of bouds
                {
                    if (i >= len1)
                        isFirstMin = false; 
                    else
                        isFirstMin = true;
                }

                if (isFirstMin)
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



        public Task ReceiveTaskFromMain() 
        {
            return comm.Receive<Task>(0, 0);
        }

        public void SendAnswerToMain(Answer ans) 
        {
            comm.Send<Answer>(ans, 0, 0);
        }

        public int[] SolveTask(Task task) 
        {
            return MergeSort(task.arr1, task.arr2);
        }
    }
}
