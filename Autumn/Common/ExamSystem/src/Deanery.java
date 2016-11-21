import java.util.ArrayList;
import java.util.Random;

public class Deanery implements Runnable {
    ExamSystem es;
    public Group[] groups;
    public ArrayList<long[]> availablePrograms;
    static int groupCount = 100;

    public Deanery(ExamSystem es) {
        this.es = es;
    }

    public void run() {
        Random rnd = new Random();

        availablePrograms = new ArrayList<>(10);
        for (int j = 0; j < 10; j++) {
            long program[] = new long[20]; // year! 20 exams!
            for (int i = 0; i < 20; i++)
                program[i] = rnd.nextLong();
            availablePrograms.add(j, program);
        }

        groups = new Group[groupCount];
        for (int i = 0; i < groupCount; i++)
            groups[i] = new Group(17 + rnd.nextInt(10), availablePrograms.get(rnd.nextInt(10)), es);

        Thread[] threads = new Thread[groupCount];
        for (int i = 0; i < groupCount; i++) {
            threads[i] = new Thread(groups[i]);
            threads[i].start();
        }

        for (Group group: groups) {
            for (long student: group.getStudents()) {
                int failed = 0;
                for (long exam: group.getCourses()) {
                    if (!es.contains(student, exam))
                        failed += 1;
                }
                if (failed >= 3)
                    System.out.println("Student " + student + " failed to pass!");
            }
        }

        for (Thread t: threads) {
            try {
                t.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }
}
