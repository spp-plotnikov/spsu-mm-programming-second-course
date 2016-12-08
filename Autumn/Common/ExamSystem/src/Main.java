import java.util.Random;


public class Main {
    final static int count = 2000;
    final static int studentCount = 1000;
    final static int coursesCount = 50;

    public static void main(String[] args) throws Exception {
        ExamSystem systems[] = { new FirstExamSystem(), new SecondExamSystem() };

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
