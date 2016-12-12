using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab5
{
    class RecursiveSum : IArraySum
    {
        public int Sum(int[] data)
        {
            return GetSum(data);
        }

        private int GetSum(int[] data)
        {
            if (data.Length != 1)
            {
                List<int> leftPart = new List<int>();
                List<int> rightPart = new List<int>();
                for (int i = 0; i < data.Length / 2; i++)
                {
                    leftPart.Add(data[i]);
                }
                for (int i = data.Length / 2; i < data.Length; i++)
                {
                    rightPart.Add(data[i]);
                }
                int leftSum = Task.Run(() => GetSum(leftPart.ToArray())).Result;
                int rightSum = Task.Run(() => GetSum(rightPart.ToArray())).Result;
                return leftSum + rightSum;
            }
            else
            {
                return data[0];
            }
        }
    }
}
