using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class ArrayActions
    {
        public static int[] GetArrayPart(int[] array, int startPos, int endPos)
        {
            List<int> newArray = new List<int>();
            for (int i = startPos; i < endPos; i++)
            {
                newArray.Add(array[i]);
            }
            return newArray.ToArray();
        }

        public static int[] MergeArrays(int[] array1, int[] array2)
        {
            return array1.Concat(array2).ToArray();
        }

        public static void SortArrayPart(ref int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i % 2 == 0)
                {
                    for (int j = 1; j < array.Length - 1; j += 2)
                    {
                        if (array[j] > array[j + 1])
                        {
                            int temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < array.Length - 1; j += 2)
                    {
                        if (array[j] > array[j + 1])
                        {
                            int temp = array[j];
                            array[j] = array[j + 1];
                            array[j + 1] = temp;
                        }
                    }
                }
            }
        }

        public static int[] Reader(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(fileName);
                string data = reader.ReadToEnd();
                reader.Close();
                if (data.Split(' ').Length < 1)
                {
                    return null;
                }
                return data.Split(' ').Select(int.Parse).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Writer(int[] array, string fileName)
        {
            try
            {
                StreamWriter writer = new StreamWriter(fileName);
                for (int i = 0; i < array.Length; i++)
                {
                    writer.Write(array[i] + " ");
                }
                writer.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
