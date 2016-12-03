using System;

namespace Deanery
{
    class Program
    {
        static void Main(string[] args)
        {
            // Testing on 1000 requests (900 for consisting, 90 - adding, 10 - removing)
            TestingSystem testingSystem = new TestingSystem(new SimpleImplementation(), new NotTrivialImplementation());
            testingSystem.StartTestOfSimple();
            testingSystem.StartTestOfNotTrivial();
            Console.ReadKey();
        }
    }
}
