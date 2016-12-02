using System;
using System.Threading;

namespace Deanery
{
    class Program
    {
        static void Main(string[] args)
        {
            TestingSystem testingSystem = new TestingSystem(new SimpleImplementation(), new NotTrivialImplementation());
            testingSystem.StartTestOfSimple();
            Thread.Sleep(2000);
            testingSystem.StartTestOfNotTrivial();
        }
    }
}
