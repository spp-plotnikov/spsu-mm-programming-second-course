using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Reflection.Emit;

namespace VirusDllCopy
{
    class Program
    {
        static void Main(string[] args)
        {

            Assembly a = Assembly.Load("CopyDll");
            Type t = a.GetType("CopyDll.Class1");
           // MethodInfo mi = t.GetMethod("Created");

            string path = Assembly.GetExecutingAssembly().Location;

            string nameFolder = String.Copy(path);
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '\\')
                {
                    nameFolder = nameFolder.Remove(i + 1, path.Length - i - 1);
                    break;
                }
            }
            Directory.CreateDirectory(nameFolder + "new\\");
            File.Copy(nameFolder + "VirusDllCopy.exe", Path.Combine(nameFolder + "new\\", "VirusDllCopy.exe"), true);

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("CopyDll"), AssemblyBuilderAccess.Save, "// nameFolder" + "new\\");
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("CopyDll", "CopyDll.dll");
            var typeBuilder = moduleBuilder.DefineType( "CopyDll.dll", TypeAttributes.Public);

            var methodBuilder = typeBuilder.DefineMethod(
                      "Main",
                      MethodAttributes.Static,
                      typeof(void),
                      null);

            var generator = methodBuilder.GetILGenerator();
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ldstr, nameFolder + "new\\");
            generator.Emit(OpCodes.Call, t.GetMethod("Created"));
            generator.Emit(OpCodes.Ret);

            typeBuilder.CreateType();
            assemblyBuilder.SetEntryPoint(methodBuilder, PEFileKinds.ConsoleApplication);
            assemblyBuilder.Save("CopyDll.dll");

        }
    }
}
