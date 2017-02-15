using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decanat
{
    public interface IExamSystem
    {
         void Add(long studentID, long courseID, job job);

        void Remove(long studentID, long courseID, job job);

        bool Contains(long studentID, long courseID);
    }
}
