using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace hashTableRef
{
    public class HashTable<T>
    {
        public int timeTable;  
        List<object> strongRef;
        const int size = 10;
        List<WeakReference>[] hashTable = new List<WeakReference>[size];

        public HashTable(int time)
        {
            timeTable = time;
            strongRef = new List<object>();
            for (int i = 0; i < 10; i++)
                hashTable[i] = new List<WeakReference>();
        }
         public void Add(T value)
        {
            hashTable[value.GetHashCode() % size].Add(new WeakReference((object)value));

            Console.WriteLine(value.ToString(),"add");
            strongRef.Add((object)value);

            TimerCallback timerCallBack = new TimerCallback(deleteStrongRef);
            Timer timer = new Timer(timerCallBack, (object)value, timeTable, Timeout.Infinite);

         }
         private void deleteStrongRef(object obj)
        {
            strongRef[(int)obj] = null;
        }

        public void Find(T obj)
        {
            foreach (WeakReference weakRef in hashTable[obj.GetHashCode() % size])
            {
                if (weakRef.IsAlive)
                {
                    if (weakRef.Target.Equals(obj))
                    {
                        Console.WriteLine(obj.ToString(),"found");
                        return;
                    }
                }
            }
            Console.WriteLine(obj.ToString(), "not found");
        }

        public void Delete(T obj)
        {
            foreach (WeakReference weakRef in hashTable[obj.GetHashCode() % size])
            {
                if (weakRef.IsAlive)
                {
                    if (weakRef.Target.Equals(obj))
                    {
                        weakRef.Target = null;
                        return;
                    }
                }
            }

            Console.WriteLine(obj.ToString(), "delete");
        }

    }
}
