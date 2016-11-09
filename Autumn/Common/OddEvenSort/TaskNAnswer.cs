using System;

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
}
