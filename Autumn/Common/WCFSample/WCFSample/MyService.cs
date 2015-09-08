using System;
using WCFContracts;

namespace WCFSample
{
    internal class MyService : IMyService
    {
        public string SayHello(string name)
        {
            Console.WriteLine(name + " called");

            return "Hello, " + name + "!";
        }
    }
}
