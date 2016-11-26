using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystem
{
    class CreditItem //лист системы
    {
        public long hash;
        public bool passed;
        public long studentID, courseID;

        public CreditItem(long studentID, long courseID)
        {
            this.courseID = courseID;
            this.studentID = studentID;
            hash = studentID.GetHashCode() + courseID;
        }
    }
}
