import sun.awt.Mutex;

import java.util.Random;

public class Main {
    public final static int count = 1000000;
    public final static int countAdd = (int) (count * 0.09);
    public final static int countRemove = (int) (count * 0.01);
    public final static int countContains = (int) (count * 0.9);
    public static Random random;
    public static final int countThread = 10;
    public static void main(String[] args) throws InterruptedException {
        System.out.println("Count threads " + countThread);
        System.out.println("Count adding " + countAdd);
        System.out.println("Count checking " + countContains);
        System.out.println("Count removing " + countRemove);
        Mutex mutex = new Mutex();
        FirstHashTable firstHashTable = new FirstHashTable(countAdd);
        SecondHashTable secondHashTable = new SecondHashTable(countAdd, mutex);
        random = new Random();
        long[] students = new long[countAdd];
        long[] courses = new long[countAdd];
        for (int i = 0; i < countAdd; i++) {
            students[i] = random.nextLong();
            courses[i] = random.nextLong();
        }

        long startTime = System.currentTimeMillis();
        Thread[] threads = new Thread[countThread];
        for (int i = 0; i < countThread; i++) {
            threads[i] = new Thread(new Tester(students, courses, firstHashTable));
            threads[i].start();
        }
        for (int i = 0; i < countThread; i++) {
            threads[i].join();
        }
        System.out.println("work time first realization: " + (System.currentTimeMillis() - startTime) / 1000.0);

        startTime = System.currentTimeMillis();
        Thread[] threadsTwo = new Thread[countThread];
        for (int i = 0; i < countThread; i++) {

            threadsTwo[i] = new Thread(new Tester(students, courses, secondHashTable));
            threadsTwo[i].start();
        }
        for (int i = 0; i < countThread; i++) {
            threadsTwo[i].join();
        }
        System.out.println("work time second realization: " + (System.currentTimeMillis() - startTime) / 1000.0);
    }

    public static class Tester implements Runnable {
        long[] students;
        long[] courses;
        IExamSystem hashTable;

        public Tester(long students[], long courses[], IExamSystem hashTable) {
            random = new Random();
            this.students = students;
            this.courses = courses;
            this.hashTable = hashTable;
        }

        public void run() {
            for (int i = 0; i < countAdd; i++) {
                hashTable.add(students[i], courses[i]);
            }

            for (int i = 0; i < countContains; i++) {
                hashTable.contains(random.nextLong(), random.nextLong());
            }

            for (int i = 0; i < countRemove; i++) {
                hashTable.remove(students[i], courses[i]);
            }
        }
    }

}
