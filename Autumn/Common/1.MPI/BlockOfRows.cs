using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class BlockOfRows
{
    public Request[] msg;
    public int[] row_k;
    public BlockOfRows(Request[] _msg, int[] _row_k)
    {
        this.msg = _msg;
        this.row_k = _row_k;
    }
}
