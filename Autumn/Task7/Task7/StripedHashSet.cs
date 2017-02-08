using System;
using System.Collections.Generic;
using System.Threading;

namespace Task7
{
    public class StripedHashSet<T>
    {
        private List<T>[] map;
        private int numberOfElements;
        private Mutex[] mutexes;
        private int threshold;

        public StripedHashSet(int size, int threshold)
        {
            map = new List<T>[size];
            for(int i = 0; i < size; i++)
            {
                map[i] = new List<T>();
            }
            numberOfElements = 0;
            mutexes = new Mutex[size];
            for(int i = 0; i < size; i++)
            {
                mutexes[i] = new Mutex();
            }
            this.threshold = threshold;
        }
        private bool Overloaded()
        {
            for(int i = 0; i < map.Length; i++)
            {
                if(map[i].Count > threshold)
                {
                    return true;
                }
            }
            return false;
        }

        private void Resize()
        {
            int currentSize = map.Length;
            for(int i = 0; i < mutexes.Length; i++)
            {
                mutexes[i].WaitOne();
            }
            try
            {
                if(currentSize != map.Length)
                {
                    return;
                }
                int newSize = currentSize * 3;
                List<T>[] data = map;
                map = new List<T>[newSize];
                for (int i = 0; i < newSize; i++)
                {
                    map[i] = new List<T>();
                }
                foreach (var list in data)
                {
                    foreach (var elem in list)
                    {
                        map[Math.Abs(elem.GetHashCode()) % map.Length].Add(elem);
                    }
                }
            }
            finally
            {
                for (int i = 0; i < mutexes.Length; i++)
                {
                    mutexes[i].ReleaseMutex();
                }
            }
        }

        private void Capture(T elem)
        {
            mutexes[(ulong)(elem.GetHashCode()) % (ulong)mutexes.Length].WaitOne();
        }

        private void Release(T elem)
        {
            mutexes[(ulong)(elem.GetHashCode()) % (ulong)mutexes.Length].ReleaseMutex();
        }
        public void Add(T elem)
        {
            Capture(elem);
            try
            {
                int hash = Math.Abs(elem.GetHashCode()) % map.Length;
                if (map[hash].Contains(elem))
                {
                    return;
                }
                else
                {
                    map[hash].Add(elem);
                    numberOfElements++;
                }
            }
            finally
            {
                Release(elem);
            }
            if (Overloaded())
            {
                Resize();
            }
        }

        public bool Contains(T elem)
        {
            Capture(elem);
            try
            {
                int hash = Math.Abs(elem.GetHashCode()) % map.Length;
                return map[hash].Contains(elem);
            }
            finally
            {
                Release(elem);
            }
        }

        public void Remove(T elem)
        {
            Capture(elem);
            try
            {
                int hash = Math.Abs(elem.GetHashCode()) % map.Length;
                if(map[hash].Contains(elem))
                {
                    map[hash].Remove(elem);
                }
                numberOfElements--;
            }
            finally
            {
                Release(elem);
            }
        }
    }
}
