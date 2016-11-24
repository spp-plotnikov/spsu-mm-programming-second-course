using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class Request
{
    public Edge ik;
    public int[] row_i;

    public Request() { }
    public Request(Edge _edge, int[] _row_i)
    {
        ik = _edge;
        row_i = _row_i;
       
    }
}


