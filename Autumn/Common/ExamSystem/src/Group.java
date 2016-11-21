import java.util.Random;

public class Group implements Runnable {
    long students[];
    long courses[];
    ExamSystem es;

    public Group(int size, long[] courses, ExamSystem es) {
        students = new long[size];
        Random rnd = new Random();
        for (int i = 0; i < size; i++)
            students[i] = rnd.nextLong();
        this.courses = courses;
        this.es = es;
    }

    public void run() {
        Random rnd = new Random();
        for (long course: courses) {
            for (long student: students) {
                if (rnd.nextInt(10) != 1)
                    es.add(student, course);
            }
        }
    }

    public long[] getStudents() {
        return students;
    }

    public long[] getCourses() {
        return courses;
    }
}
