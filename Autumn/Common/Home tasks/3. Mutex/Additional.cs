using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Producer_consumer
{
    class Additional
    {
        private Mutex mtx;
        private List<Consumer> consumerList;
        private List<Producer> producerList;
        private int numConsumers;
        private int numProducers;
        private List<int> buf;

        public Additional(int numCons, int numProd)
        {
            consumerList = new List<Consumer>();
            producerList = new List<Producer>();
            buf = new List<int>();
            numConsumers = numCons;
            numProducers = numProd;
            mtx = new Mutex();
        }

        private void addProducer(int i)
        {
            Producer newProducer = new Producer(i, this);
            producerList.Add(newProducer);
            newProducer.Start();
        }

        private void addConsumer(int i)
        {
            Consumer newConsumer = new Consumer(i, this);
            consumerList.Add(newConsumer);
            newConsumer.Start();
        }

        public void Add()
        {
            for (int i = 1; i <= numProducers; i++)
            {
                addProducer(i);
            }

            for (int i = 1; i <= numConsumers; i++)
            {
                addConsumer(i);
            }
        }

        public void BufEnque(int x)
        {
            buf.Add(x);
        }

        public int BufDeque()
        {
            int x = buf.First();
            buf.Remove(x);
            return x;
        }

        public int GetBufSize()
        {
            return buf.Count();
        }

        public void MtxWait()
        {
            mtx.WaitOne();
        }

        public void MtxRelease()
        {
            mtx.ReleaseMutex();
        }

        public void Close()
        {
            for (int i = 1; i <= numProducers; i++)
            {
                producerList[0].Stop();
                producerList[0].ThreadJoin();
                producerList.RemoveAt(0);
            }
            for (int i = 1; i <= numConsumers; i++)
            {
                consumerList[0].Stop();
                consumerList[0].ThreadJoin();
                consumerList.RemoveAt(0);
            }
        }

    }
}
