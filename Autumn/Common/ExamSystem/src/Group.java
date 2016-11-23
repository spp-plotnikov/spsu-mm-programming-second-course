import java.util.LinkedList;
import java.util.Random;

public class Group implements Runnable {
    long students[];
    long courses[];
    LinkedList<Long> expelled;
    ExamSystem es;

    public Group(int size, long[] courses, ExamSystem es) {
        students = new long[size];
        Random rnd = new Random();
        for (int i = 0; i < size; i++)
            students[i] = rnd.nextLong();
        this.courses = courses;
        this.es = es;
        expelled = new LinkedList<>();
    }

    // Trying to survive
    public void run() {
        Random rnd = new Random();
        for (long course: courses) {
            for (long student: students) {
                // have to expel more students to have stats good (less add queries)
                if (rnd.nextInt(10) > 4)
                    es.add(student, course);
            }
        }
    }

    public void expel(long student) {
        expelled.add(student);
    }
    public long[] getStudents() {
        return students;
    }

    public long[] getCourses() {
        return courses;
    }
}
