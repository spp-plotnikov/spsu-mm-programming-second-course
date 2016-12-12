using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab5
{
    public class ParticularSum : IArraySum
    {
        public int Sum(int[] data)
        {
            return GetSum(data);
        }

        private int GetParticularSum(int[] data, int leftBoard, int rightBoard)
        {
            int sum = 0;
            for (int i = leftBoard; i < rightBoard; i++)
            {
                sum += data[i];
            }
            return sum;
        }

        private int GetSum(int[] data)
        {
            int sum = 0;
            List<int> listOfSum = new List<int>();
            int partOfArray = data.Length / 250;
            for (int i = 0; i < 249; i++)
            {
                listOfSum.Add(Task.Run(() => GetParticularSum(data, i * partOfArray, (i + 1) * partOfArray)).Result);

            }
            listOfSum.Add(Task.Run(() => GetParticularSum(data, 249 * partOfArray, data.Length)).Result);
            listOfSum.ForEach(i => { sum += i; });
            return sum;
        }
    }
}
