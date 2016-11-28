using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Producer
    {
        private Thread _thread;
        private readonly int _name;
        private List<int> _goods;
        private Random _rnd = new Random();
        private bool _state = true;

        public Producer(int name, List <int> goods)
        {
            _name = name;
            _goods = goods;
            _thread = new Thread(this.Run);
            _thread.Start();
        }

        public void Finish()
        {
            _state = false;
            _thread.Join();
        }

        void Run()
        {
            while (_state) AddSomething();
        }

        public void AddSomething()
        {
            int toPut;
            Monitor.Enter(_goods);
            toPut = _rnd.Next(0, 1000);
            Console.WriteLine("Producer number {0} added to list {1}", _name, toPut);
            _goods.Add(toPut);
            Monitor.Pulse(_goods);
            Monitor.Exit(_goods);
            Thread.Sleep(400);
        }
    }
}
