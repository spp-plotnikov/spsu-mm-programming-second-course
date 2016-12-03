using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Consumer_Producer
{
    public class Conveyor<T> : IGetable<T>, IPutable<T>
    {
        private List<T> _conveyor;
        private object _syncObj = new object();

        public Conveyor()
        {
            _conveyor = new List<T>();
        } 

        public T Get()
        {
            Monitor.Enter(_syncObj);

            while (_conveyor.Count == 0)
            {
                Monitor.Wait(_syncObj);
            }

            T obj;

            try
            {
                obj = _conveyor.First();
                _conveyor.Remove(obj);
                Console.WriteLine(obj.ToString() + " was removed by {0} thread", Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                Monitor.PulseAll(_syncObj);
                Monitor.Exit(_syncObj);
            }
            return obj;
        }

        public void Put(T obj)
        {
            
            Monitor.Enter(_syncObj);
            try
            {
                _conveyor.Add(obj);
                Console.WriteLine(obj.ToString() + " was added by {0} thread", Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                Monitor.PulseAll(_syncObj);
                Monitor.Exit(_syncObj);
            }
        }
    }
}