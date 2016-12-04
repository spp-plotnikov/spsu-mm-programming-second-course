using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSystems
{
    class CreditItem : ListItem //лист системы
    {
        public int hash;
        public bool passed;
        public long studentID, courseID;

        public CreditItem(long studentID, long courseID)
        {
            this.courseID = courseID;
            this.studentID = studentID;
            hash = Convert.ToInt32(studentID.GetHashCode() + courseID);
            bin = ToBinary(hash);
        }
    }
}
