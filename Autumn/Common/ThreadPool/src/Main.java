import java.util.concurrent.ThreadLocalRandom;

public class Main {
    public static void main(String[] args) throws Exception {
        MyThreadPool pool = new MyThreadPool();
        pool.start();
        long st = System.currentTimeMillis();
        for (int i = 0; i < 10; i++) {
            Runnable task = () -> {
                int me = ((int) Thread.currentThread().getId());// % MyThreadPool.threadMax + 1);
                try {
                    System.out.println("[" + (System.currentTimeMillis() - st) + "] Task " + me + ": started");
                    Thread.sleep(ThreadLocalRandom.current().nextInt(100, 5000));
                    System.out.println("[" + (System.currentTimeMillis() - st) + "] Task " + me + ": finished");
                }
                catch (InterruptedException e) {
                    System.out.println("[" + (System.currentTimeMillis() - st) + "] Task " + me + ": terminated");
                }
            };
            pool.enqueue(task);
            // for the sake of emulation...
            Thread.sleep(ThreadLocalRandom.current().nextInt(200, 400));
        }
        Thread.sleep(5000); // give 'em some time
        pool.stop();
        System.out.println("Program has finished");
    }
}
