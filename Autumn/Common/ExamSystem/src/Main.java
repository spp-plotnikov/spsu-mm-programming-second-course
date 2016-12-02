import java.util.Random;

class ExamSystemTester implements Runnable {
    private long[] students;
    private long[] courses;
    private ExamSystem es;
    private Random rnd = new Random(System.currentTimeMillis());

    public ExamSystemTester(ExamSystem es, long[] students, long[] courses) {
        this.es = es;
        this.students = students;
        this.courses = courses;
    }

    public void run() {
        for (int i = 0; i < 100; i++) {
            int j = rnd.nextInt(students.length);
            int k = rnd.nextInt(courses.length);
            es.add(students[j], courses[k]);
        }

        for (int i = 0; i < 1000; i++) {
            int j = rnd.nextInt(students.length);
            int k = rnd.nextInt(courses.length);
            es.contains(students[j], courses[k]);
        }

        for (int i = 0; i < 10; i++) {
            int j = rnd.nextInt(students.length);
            int k = rnd.nextInt(courses.length);
            es.remove(students[j], courses[k]);
        }
    }
}

public class Main {
    final static int size = 10000000; // if crashes, it means we have to enlarge it
    final static int count = 20;
    final static int studentCount = 500;
    final static int coursesCount = 25;

    public static void main(String[] args) throws Exception {
        ExamSystem systems[] = { new FirstExamSystem(size), new SecondExamSystem(size) };

        Random rnd = new Random();
        long[] students = new long[studentCount];
        long[] courses = new long[coursesCount];
        for (int i = 0; i < studentCount; i++)
            students[i] = rnd.nextLong();
        for (int i = 0; i < coursesCount; i++)
            courses[i] = rnd.nextLong();

        for (ExamSystem es: systems) {
            System.out.println("System type: " + es.getClass());
            long startTime = System.currentTimeMillis();
            Thread threads[] = new Thread[count];
            for (int i = 0; i < count; i++) {
                ExamSystemTester tester = new ExamSystemTester(es, students, courses);
                threads[i] = new Thread(tester);
                threads[i].start();
            }
            for (Thread t: threads)
                t.join();
            System.out.println(es.getStats());
            System.out.println("== Elapsed: " + (System.currentTimeMillis() - startTime) + "ms ==");
        }
    }
}
