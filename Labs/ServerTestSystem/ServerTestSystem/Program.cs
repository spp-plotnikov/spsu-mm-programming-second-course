using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTestSystem
{
    class Program
    {       

        static void Main(string[] args) 
        {
            var functions = new TestFunctions();
            //functions.MaxClients();
            //functions.MediumAndMediana();
            Console.WriteLine("Finita");
            Console.ReadKey();
        }    


    }
}
