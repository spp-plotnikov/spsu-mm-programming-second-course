import java.util.Random;

public class Test implements Runnable {
    private IExamSystem storage;

    Test(IExamSystem storage) {
        this.storage = storage;
    }

    @Override
    public void run() {
        long action = 1;
        while (action < 1000000) {
            Random random = new Random();

            long studentId = (long)random.nextInt(1000);
            long courseId = (long)random.nextInt(100);

            if (action % 10 != 0) {
                storage.contains(studentId, courseId);
            }

            if (action % 100 == 0) {
                storage.remove(studentId, courseId);
            }

            if (action % 100 < 9) {
                storage.add(studentId, courseId);
            }

            action++;
        }
    }
}
