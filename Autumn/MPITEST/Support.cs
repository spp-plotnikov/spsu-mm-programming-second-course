using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPITEST
{
    [Serializable]
    public class Task
    {
        public int[] arr1;
        public int[] arr2;
        public int firstIndex;

        public Task(int[] a1, int[] a2, int f)
        {
            arr1 = a1;
            arr2 = a2;
            firstIndex = f;
        }
    }

    [Serializable]
    public class Answer
    {
        public int[] arr;
        public int firstIndex;

        public Answer(int[] a, int f)
        {
            arr = a;
            firstIndex = f;
        }
    }
}
