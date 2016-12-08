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
