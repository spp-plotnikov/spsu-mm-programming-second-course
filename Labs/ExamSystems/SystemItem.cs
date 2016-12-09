using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystems
{
    class SystemItem : ListItem //лист системы
    {
        public int Hash;
        public bool IsPassed;
        public long StudentID, CourseID;

        public SystemItem(long studentID, long courseID)
        {
            CourseID = courseID;
            StudentID = studentID;
            Hash = Convert.ToInt32(studentID.GetHashCode() + courseID);
            Bin = ToBinary(Hash);
        }
    }
}
