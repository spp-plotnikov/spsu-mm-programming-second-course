﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Path to config:");
            BmpServer server = new BmpServer(13000, Console.ReadLine());
        }
    }
}
