/*
    Disclaimer:
    No identification with actual persons (living or deceased), places,
    buildings, and products is intended or should be inferred.
*/

import java.util.Random;
import java.util.concurrent.ThreadLocalRandom;

public class AngryRectorate implements Runnable {
    ExamSystem es;
    Deanery[] deaneries;

    public AngryRectorate(ExamSystem es) {
        this.es = es;
    }

    // Starts this hell and tries to spoil everything
    public void run() {
        Random rnd = new Random();

        deaneries = new Deanery[27];
        Thread[] threads = new Thread[deaneries.length];
        for (int i = 0; i < threads.length; i++) {
            deaneries[i] = new Deanery(es);
            threads[i] = new Thread(deaneries[i]);
            threads[i].start();
        }

        try {
            Thread.sleep(100); // day off
        } catch (InterruptedException e) {
            e.printStackTrace();
        }

        // Oops, according to recent inspection we have to revoke several records
        int count = rnd.nextInt(100);
        for (int i = 0; i < count; i++) {
            int r1 = rnd.nextInt(deaneries.length);
            int r2 = rnd.nextInt(deaneries[r1].groups.length);
            Group luckyGroup = deaneries[r1].groups[r2];
            int r3 = rnd.nextInt(luckyGroup.getStudents().length);
            int r4 = rnd.nextInt(luckyGroup.getCourses().length);
            es.remove(luckyGroup.getStudents()[r3], luckyGroup.getCourses()[r4]);
        }

        int expelled = 0, total = 0;
        try {
            for (int i = 0; i < deaneries.length; i++) {
                threads[i].join();
                for (Group g: deaneries[i].groups) {
                    expelled += g.expelled.size();
                    total += g.getStudents().length;
                }
            }
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        System.out.println("Expelled: " + expelled + " out of " + total + " )))");
    }
}
