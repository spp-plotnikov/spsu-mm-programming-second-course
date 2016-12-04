using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    class MyService: IMyService
    {
        public List<string> Filters;
        public string SayHello(string name)
        {
            Console.WriteLine("Request: "+ name);
            return "Hello, " + name;
        }
        public List<string> GetFilters()
        {
            return null;
        }
    }
}
