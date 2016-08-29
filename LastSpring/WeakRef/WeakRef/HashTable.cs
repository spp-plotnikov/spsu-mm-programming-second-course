using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeakRef
{
    class HashTable<T>
    {
        public int StrongRefTime { get; private set; }
        private int variable;
        const int tableSize = 3;
        List<WeakReference>[] hashTable;
        List<object> strongRef;

        public HashTable(int time)
        {
            variable = 0;
            StrongRefTime = time;
            hashTable = new List<WeakReference>[tableSize];
            for (int i = 0; i < 3; i++)
            {
                hashTable[i] = new List<WeakReference>();
            }
            strongRef = new List<object>();
        }

        public void Add(T data)
        {
            variable++;

            object obj = data;

            int hash = obj.GetHashCode() % tableSize;
            WeakReference weakRef = new WeakReference(obj);
            hashTable[hash].Add(weakRef);

            strongRef.Add(obj);

            if (hash % 2 == 0)
            {
                TimerCallback timerCallBack = new TimerCallback(RemoveStrongRef);
                Timer timer = new Timer(timerCallBack, variable, StrongRefTime, Timeout.Infinite);
            }
        }

        public bool Find(T obj)
        {
            int hash = obj.GetHashCode() % tableSize;
            foreach (WeakReference weakRef in hashTable[hash])
            {
                if (weakRef.IsAlive)
                    if (weakRef.Target.Equals(obj))
                        return true;
            }
            return false;
        }

        public void Delete(T obj)
        {
            int hash = obj.GetHashCode() % tableSize;
            foreach (WeakReference weakRef in hashTable[hash])
            {
                if (weakRef.IsAlive)
                    if (weakRef.Target.Equals(obj))
                    {
                        weakRef.Target = null;
                        return;
                    }
            }
            Console.WriteLine(obj.ToString() + " Not found");
        }
        
        private void RemoveStrongRef(object variable)
        {
            int pos = (int)variable;
            strongRef[pos - 1] = null;
        }
    }
}
