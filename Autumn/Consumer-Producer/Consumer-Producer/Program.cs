using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Consumer_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var cnv = new Conveyor<string>();

            List<Producer<string>> producers =
                Enumerable.Range(0, 5).Select(i => new Producer<string>(200, rnd.Next(), cnv)).ToList();
            List<Consumer<string>> consumers =
                Enumerable.Range(0, 10).Select(i => new Consumer<string>(200, cnv)).ToList();

            Console.WriteLine("FACTORY IS ON!!! Press any key to stop it");

            producers.ForEach(i => i.StartProducing());
            consumers.ForEach(i => i.StartConsuming());

            Console.ReadKey();

            producers.ForEach(i => i.StopProducing());
            consumers.ForEach(i => i.StopConsuming());
        }
    }
}
