using System;

namespace UniversityRequest.ExamSystems
{
    public struct SystemRecord
    {
        public long StudentId { get; }
        public long CourseId { get; }

        public SystemRecord(long studentId, long courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }

        public bool Equals(SystemRecord other)
        {
            return StudentId == other.StudentId && CourseId == other.CourseId;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Math.Abs(StudentId.GetHashCode()*397 ^ CourseId.GetHashCode());
            }
        }
    }
}