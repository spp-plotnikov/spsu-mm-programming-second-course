using System.Linq;
using System.Threading.Tasks;

namespace Task5
{
    public class ArraySum : IArraySum
    {
        private const int partition = 10;
        public int Sum(int[] a)
        {
            int[][] parts = new int[partition][];
            Task<int>[] tasks = new Task<int>[partition];
            for(int i = 0; i < partition; i++)
            {
                int j = i;
                parts[i] = a.Where((value, index) => index % partition == i).ToArray();
                tasks[i] = Task.Run(() => parts[j].Sum());
            }
            Task.WaitAll(tasks);
            int sum = 0;
            foreach(var task in tasks)
            {
                sum += task.Result;
            }
            return sum;
        }
    }
}
