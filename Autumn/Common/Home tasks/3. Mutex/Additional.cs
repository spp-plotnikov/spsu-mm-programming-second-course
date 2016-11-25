using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Producer_consumer
{
    class Additional
    {
        private List<Consumer> consumerList;
        private List<Producer> producerList;
        private int numConsumers;
        private int numProducers;
        private Buffer buffer;

        public Additional(int numCons, int numProd, Buffer buf)
        {
            consumerList = new List<Consumer>();
            producerList = new List<Producer>();
            numConsumers = numCons;
            numProducers = numProd;
            buffer = buf;
        }

        private void addProducer(int i)
        {
            Producer newProducer = new Producer(i, buffer);
            producerList.Add(newProducer);
            newProducer.Start();
        }

        private void addConsumer(int i)
        {
            Consumer newConsumer = new Consumer(i, buffer);
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
