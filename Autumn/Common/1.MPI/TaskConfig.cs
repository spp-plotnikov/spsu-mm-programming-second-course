using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
class TaskConfig
{
    public bool Runnable = true;
    public int Size;
    public int BlockSize;
    public TaskConfig() { }
    public TaskConfig(int _Size, int _BlockSize)
    {
        this.Size = _Size;
        this.BlockSize = _BlockSize;
    }
    public TaskConfig(bool flag)
    {
        this.Runnable = flag;
    }
}