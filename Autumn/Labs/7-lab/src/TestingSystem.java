public class TestingSystem {
    private IExamSystem storage;
    private Thread[] threads;
    private int threadsCount;

    TestingSystem(IExamSystem storage, int threadsCount) {
        this.storage = storage;

        this.threads = new Thread[threadsCount];
        this.threadsCount = threadsCount;
    }

    public long start() {
        long timeStart = System.currentTimeMillis();

        for (int i = 0; i < threadsCount; i++) {
            threads[i] = new Thread(new Test(storage));
            threads[i].start();
        }

        for (int i = 0; i < threadsCount; i++) {
            try {
                threads[i].join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }

        return System.currentTimeMillis() - timeStart;
    }
}
