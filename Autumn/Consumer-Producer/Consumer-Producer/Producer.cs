using System;
using System.Threading;

namespace Consumer_Producer
{
    /// <summary>
    /// string producer, simply changable to other types
    /// </summary>
    /// <typeparam name="T">type of producing items</typeparam>
    public class Producer<T>
    {
        private T _item;
        private bool _canProduce;

        public int Pause { get; }
        public IPutable<T> Conveyor { get; }

        public Producer(int pause, int id, IPutable<T> conveyor)
        {
            Pause = pause;
            _item = GetProducingItem(id);
            Conveyor = conveyor;
            _canProduce = true;
        }

        public void StartProducing()
        {
            Thread thread = new Thread(() =>
            {
                while (_canProduce)
                {
                    Conveyor.Put(_item);
                    Thread.Sleep(Pause);
                }
            });
            thread.Start();
        }

        public void StopProducing()
        {
            _canProduce = false;
            Conveyor.Stop();
        }

        private T GetProducingItem(int id)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(id.ToString(), typeof(T));
            }
            else
            {
                return default(T);
            }
        }
    }
}