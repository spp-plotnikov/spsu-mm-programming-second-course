import java.util.ArrayList;
import java.util.Random;

/*
    Disclaimer:
    No identification with actual persons (living or deceased), places,
    buildings, and products is intended or should be inferred.
*/

public class Deanery implements Runnable {
    ExamSystem es;
    public Group[] groups;
    public ArrayList<long[]> availablePrograms;
    static int groupCount = 100;

    public Deanery(ExamSystem es) {
        this.es = es;
    }

    // Deanery rules the process among groups
    public void run() {
        Random rnd = new Random();

        // First we need some random study programs (nobody cares anyway)
        availablePrograms = new ArrayList<>(10);
        for (int j = 0; j < 10; j++) {
            long program[] = new long[20]; // year! 20 exams, excellent!
            for (int i = 0; i < 20; i++)
                program[i] = rnd.nextLong();
            availablePrograms.add(j, program);
        }

        // Then some random people...
        groups = new Group[groupCount];
        for (int i = 0; i < groupCount; i++)
            groups[i] = new Group(17 + rnd.nextInt(10), availablePrograms.get(rnd.nextInt(10)), es);

        // Here the exam session begins!
        Thread[] threads = new Thread[groupCount];
        for (int i = 0; i < groupCount; i++) {
            threads[i] = new Thread(groups[i]);
            threads[i].start();
        }

        // Unfortunately unable to expel 'em before they finish
        for (Thread t: threads) {
            try {
                t.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        // Okay, now it's time to weed out some people
        for (Group group: groups) {
            for (long student: group.getStudents()) {
                int failed = 0;
                for (long exam: group.getCourses()) {
                    if (!es.contains(student, exam))
                        failed += 1;
                }
                if (failed >= 3)
                    group.expel(student);
                    //System.out.println("Student " + student + " have been expelled))))");
            }
        }
    }
}
