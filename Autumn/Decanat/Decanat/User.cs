using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decanat
{
    class User
    {
        List<long> coursesID = new List<long>();
        List<long> studentsID = new List<long>();
        readonly long courseID;
        readonly long studentID;
        readonly job job;
        private IExamSystem system;
        
        /// <param name="list">list students or courses depending on the job</param>
        public User(job job, long ID, List<long> list, IExamSystem system)
        {
            this.job = job;
            if (job == job.Student)
            {
                studentID = ID;
                coursesID = list;
            }
            else
            {
                courseID = ID;
                studentsID = list;
            }
            this.system = system;
        }

        public void AddExam(int numInList)
        {
            if (job == job.Student)
                Task.Run(() => system.Add(studentID, coursesID[numInList], job));
            else
                Task.Run(() => system.Add(studentsID[numInList], courseID, job));
        }
        
        public void RemoveExam(int numInList)
        {
            if (job == job.Student)
                Task.Run(() => system.Remove(studentID, coursesID[numInList], job));
            else
                Task.Run(() => system.Remove(studentsID[numInList], courseID, job));
        }

        public bool ContainsExam(long courseID, long studentID)
        {
            var task = Task.Run(() => system.Contains(studentID, courseID));
            return task.Result;
        }
    }
}
