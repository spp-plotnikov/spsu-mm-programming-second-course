using System;

namespace Deanery
{
    class Credit : IEquatable<Credit>
    {
        public long studentId;
        public long courseId;
        public Credit(long studentId, long courseId)
        {
            this.studentId = studentId;
            this.courseId = courseId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Credit);
        }

        public bool Equals(Credit obj)
        {
            return obj != null && obj.courseId == this.courseId && obj.studentId == this.studentId;
        }

        public static bool operator ==(Credit fst, Credit snd)
        {
            return Equals(fst, snd);
        }

        public static bool operator !=(Credit fst, Credit snd)
        {
            return !(Equals(fst, snd));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (int)(this.courseId + this.studentId);
            }
        }
    }
}
