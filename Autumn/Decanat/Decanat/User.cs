using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decanat
{
    class User
    {
        private List<long> _coursesID = new List<long>();
        private List<long> _studentsID = new List<long>();
        readonly long courseID;
        readonly long studentID;
        readonly job job;
        private IExamSystem _system;
        
        /// <param name="list">list students or courses depending on the job</param>
        public User(job job, long ID, List<long> list, IExamSystem system)
        {
            this.job = job;
            if (job == job.Student)
            {
                studentID = ID;
                _coursesID = list;
            }
            else
            {
                courseID = ID;
                _studentsID = list;
            }
            _system = system;
        }

        public void AddExam(int numInList)
        {
            if (job == job.Student)
                Task.Run(() => _system.Add(studentID, _coursesID[numInList], job));
            else
                Task.Run(() => _system.Add(_studentsID[numInList], courseID, job));
        }
        
        public void RemoveExam(int numInList)
        {
            if (job == job.Student)
                Task.Run(() => _system.Remove(studentID, _coursesID[numInList], job));
            else
                Task.Run(() => _system.Remove(_studentsID[numInList], courseID, job));
        }

        public bool ContainsExam(long courseID, long studentID)
        {
            var task = Task.Run(() => _system.Contains(studentID, courseID));
            return task.Result;
        }
    }
}
