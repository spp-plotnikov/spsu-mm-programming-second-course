using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExamSystem
{
    class ListItem
    {
        public Mutex BucketMutex = new Mutex();
        public ListItem Next = null;
        public string BinFormat;
        public bool IsBucket = false;

        protected string ToBinary(long number)
        {
            long n = number;
            string s = "";
            int k = 16;
            while (n != 0)
            {
                s += Convert.ToString(n % 2);
                n = n / 2;
                k--;
            }
            while (k > 0)
            {
                s += "0";
                k--;
            }
            return s;
        }
    }

    class Bucket : ListItem
    {
        public int N;
        public Bucket(int n)
        {
            N = n;
            BinFormat = ToBinary(n);
            IsBucket = true;
        }
    }

    class StudInfo : ListItem
    {
        public int Hash;
        public bool IsPassed;
        public long StudentID, CourseID;

        public  StudInfo    (long studentID, long courseID)
        {
            CourseID = courseID;
            StudentID = studentID;
            Hash = Convert.ToInt32(studentID.GetHashCode() + courseID);
            BinFormat = ToBinary(Hash);
        }
    }
}

