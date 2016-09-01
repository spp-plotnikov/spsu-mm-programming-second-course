using System;
using System.Diagnostics;
using System.IO;

namespace ReflectionEmit
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            string[] fileDirs;

            if (!File.Exists("BmpLibrary.dll"))
            {
                Console.WriteLine("Cannot find dll!!");
                return;
            }

            Console.Write("Path: ");

            path = Console.ReadLine();
            if (Directory.Exists(path))
            {
                fileDirs = Directory.GetFiles(path, "*.bmp");
                if (fileDirs.Length == 0)
                {
                    Console.WriteLine("There is no *.bmp in {0}", path);
                    return;
                }
            }
            else
            {
                Console.WriteLine("Path isn't exist");
                return;
            }


            File.Delete(Path.Combine(path, "BmpLibrary.dll"));
            File.Copy("BmpLibrary.dll", Path.Combine(path, "BmpLibrary.dll"));

            foreach (string dir in fileDirs)
            {
                string fileName;
                for (int i = dir.Length - 1; ; i--)
                    if (dir[i] == '\\')
                    {
                        fileName = dir.Remove(0, i + 1);
                        fileName = fileName.Remove(fileName.Length - 4, 4);
                        break;
                    }

                var start = new ProcessStartInfo();

                start.WorkingDirectory = path;
                start.FileName = Generator.Generate(fileName, dir);

                Process.Start(start);

            }
            Console.WriteLine("Success, press Enter");
            Console.ReadKey();
        }
    }
}
