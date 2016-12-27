using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7
{
    public class FastSystem : IExamSystem
    {
        private StripedHashSet<KeyValuePair<long, long>> table = new StripedHashSet<KeyValuePair<long, long>>(1000, 10);

        public void Add(long studentId, long courseId)
        {
            table.Add(new KeyValuePair<long, long>(studentId, courseId));
        }

        public void Remove(long studentId, long courseId)
        {
            table.Remove(new KeyValuePair<long, long>(studentId, courseId));
        }

        public bool Contains(long studentId, long courseId)
        {
            return table.Contains(new KeyValuePair<long, long>(studentId, courseId));
        }
    }
}
