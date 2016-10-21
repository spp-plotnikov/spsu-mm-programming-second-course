using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using BmpLibraruRef;

namespace Reflection
{
    public class Program
    {
        public static void Main()
        {
            if (!File.Exists("BmpLibraruRef.dll"))
            {
                Console.WriteLine("Filter is not found");
                return;
            }
            Console.WriteLine("Enter path");
            string path = Console.ReadLine();

            string[] dirs;
            int ok = 0;

            while (ok == 0)
            {
                ok = 1;
                try
                {
                    dirs = Directory.GetFiles(@path, "*.bmp");

                }
                catch
                {
                    Console.WriteLine("Error path. Again");
                    path = Console.ReadLine();
                    ok = 0;
                }
            }

            dirs = Directory.GetFiles(@path, "*.bmp");

            foreach (string dir in dirs)
            {
                string nameProgram = String.Copy(dir);
                for (int i = dir.Length - 1; i > 0; i--)
                {
                    if (dir[i] == '\\')
                    {
                        nameProgram = nameProgram.Remove(0, i + 1);
                        break;
                    }
                }
                var name = new AssemblyName("Filter_");
                var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
                var moduleBuilder = assemblyBuilder.DefineDynamicModule(name.Name, "Filter_" + nameProgram + ".exe");
                var typeBuilder = moduleBuilder.DefineType(name.Name + nameProgram + ".exe", TypeAttributes.Public);

                var methodBuilder = typeBuilder.DefineMethod(
                    "Main",
                    MethodAttributes.Static,
                    typeof(void),
                    null);

                var generator = methodBuilder.GetILGenerator();

                generator.Emit(OpCodes.Nop);
                generator.Emit(OpCodes.Ldstr, dir);
                generator.Emit(OpCodes.Call, typeof(Filterclass).GetMethod("Filter"));
                generator.Emit(OpCodes.Ret);

                typeBuilder.CreateType();
                assemblyBuilder.SetEntryPoint(methodBuilder, PEFileKinds.ConsoleApplication);
                assemblyBuilder.Save(name.Name + nameProgram + ".exe");
            }
        }
    }
}