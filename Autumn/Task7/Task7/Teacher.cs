using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7
{
    public class Teacher
    {
        public List<KeyValuePair<long, long>> SomeInfo
        {
            get;
            private set;
        }
        public Teacher(List<KeyValuePair<long, long>> info)
        {
            SomeInfo = info;
        }

        public void Work(IExamSystem system)
        {
            Random rand = new Random();
            for(int i = 0; i < 900; i++)
            {
                int random = rand.Next(SomeInfo.Count);
                var temp = SomeInfo[random];
                system.Contains(temp.Key, temp.Value);
            }
            for (int i = 0; i < 90; i++)
            {
                int random = rand.Next(SomeInfo.Count);
                var temp = SomeInfo[random];
                system.Add(temp.Key, temp.Value);
            }
            for(int i = 0; i < 10; i++)
            {
                int random = rand.Next(SomeInfo.Count);
                var temp = SomeInfo[random];
                system.Remove(temp.Key, temp.Value);
            }
        }
    }
}
