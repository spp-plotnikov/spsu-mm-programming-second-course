import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.atomic.AtomicBoolean;

public class Main {
    static int cN = 12; // number of consumers
    static int pN = 4; // number of producers
    static ExecutorService pool;
    static MyConsumer<Integer>[] consumers = new MyConsumer[cN];
    static MyProducer<Integer>[] producers = new MyProducer[pN];

    static volatile AtomicBoolean exitFlag /* aka kostyl */ = new AtomicBoolean(false);

    // This is used as a callback for KeyCatcher
    public static class PoolDestroyer implements Runnable {
        public void run() {
            System.out.println("Shutting down!");
            exitFlag.set(true);
            pool.shutdown();
            while (!pool.isTerminated()) { } // wait
            System.out.println("Finished");
        }
    }

    public static void main(String[] args) throws Exception {
        Queue<Integer> q = new LinkedList<>();
        int n = cN + pN;
        MyMutex mutex = new MyMutex(n);
        pool = Executors.newFixedThreadPool(n);
        for (int i = 0; i < pN; i++) {
            producers[i] = new MyProducer<>(q, mutex, 5, exitFlag);
            Thread t = new Thread(producers[i]);
            pool.submit(t);
        }
        for (int i = 0; i < cN; i++) {
            consumers[i] = new MyConsumer<>(q, mutex, exitFlag);
            Thread t = new Thread(consumers[i]);
            pool.submit(t);
        }

        new KeyCatcher(new PoolDestroyer());
    }
}
