using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Filters
    {
        private static readonly string path = "config.txt";
        public static string[] GetFilters()
        {
            return File.ReadAllLines(@path);
        }
    }
}
