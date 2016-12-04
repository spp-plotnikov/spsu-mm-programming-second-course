using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    class MyService: IMyService
    {
        public List<string> GetFilters(string fileName)
        {
            string line;
            List<string> filters = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(@fileName);
            while ((line = file.ReadLine()) != null)
            {
                filters.Add(line);               
            }
            return filters;
        }
    }
}
