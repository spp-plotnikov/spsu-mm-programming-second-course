using System;
using System.Reflection;
using System.Reflection.Emit;
using BmpLibrary;

namespace ReflectionEmit
{
    public static class Generator
    {
        public static string Generate(string name, string dirRead)
        {
            var assemblyName = new AssemblyName(name);

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                assemblyName, AssemblyBuilderAccess.RunAndSave, dirRead.Replace(name + ".bmp", ""));

            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name, name + ".exe");

            var typeBuilder = moduleBuilder.DefineType(name + ".Program", TypeAttributes.Class);

            var methodBuilder = typeBuilder.DefineMethod(
                "Main",
                MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(void),
                new Type[] { typeof(string[]) });

            var gen = methodBuilder.GetILGenerator();

            gen.Emit(OpCodes.Ldstr, name + ".bmp");
            gen.Emit(OpCodes.Ldstr, "Grey_" + name + ".bmp");
            gen.Emit(OpCodes.Call, typeof(Filter).GetMethod("Grey"));
            gen.Emit(OpCodes.Ret);

            typeBuilder.CreateType();
            assemblyBuilder.SetEntryPoint(methodBuilder, PEFileKinds.ConsoleApplication);
            assemblyBuilder.Save(name + ".exe");
            return (name + ".exe");
        }
    }
}
