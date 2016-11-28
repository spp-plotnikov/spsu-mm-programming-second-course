using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    class Consumer
    {
        private Thread _thread;
        private readonly int _name;
        private  List<int> _goods;
        private Random _rnd = new Random();
        private bool _state = true;

        public Consumer(int name, List <int> goods)
        {
            _name = name;
            _goods = goods;
            _thread = new Thread(this.Run);
            _thread.Start();
        }

        public void Finish()
        {
            _state = false;
            Monitor.Enter(_goods);
            Monitor.Pulse(_goods);
            Monitor.Exit(_goods);
            _thread.Join();
        }

        void Run()
        {
            while (_state) TakeSomething();
        }

        // removes some element from the given list
        public void TakeSomething()
        {
            Monitor.Enter(_goods);

            if (_goods.Count != 0)
            {
                int toRemove = _rnd.Next(0, _goods.Count - 1);
                Console.WriteLine("Consumer number {0} removed from list element {1}", _name, _goods[toRemove]);
                _goods.Remove(_goods[toRemove]);
            }
            else
            {
                Console.WriteLine("There is nothing to remove from the list!");
                Monitor.Wait(_goods);
                if (!_state)
                {
                    Monitor.Pulse(_goods);
                    Monitor.Exit(_goods);
                    return;
                }
            }

            Monitor.Exit(_goods);
            Thread.Sleep(400);
        }
    }
}
