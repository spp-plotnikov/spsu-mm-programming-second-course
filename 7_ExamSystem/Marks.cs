using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Marks
{
    public long[] Students;
    public long[] Courses;
    public IExamSystem Results;
    public Marks(long[] students, long[] courses, IExamSystem results)
    {
        this.Students = students;
        this.Courses = courses;
        this.Results = results;
    }
}
