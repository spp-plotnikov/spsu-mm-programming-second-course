import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Main {
    static int cN = 12; // number of consumers
    static int pN = 4; // number of producers

    public static void main(String[] args) throws Exception {
        Queue<Integer> q = new LinkedList<>();
        int n = cN + pN;
        MyMutex mutex = new MyMutex(n);
        ExecutorService pool = Executors.newFixedThreadPool(n);
        for (int i = 0; i < pN; i++) {
            Thread t = new Thread(new MyProducer<>(q, mutex, 5));
            pool.submit(t);
        }
        for (int i = 0; i < cN; i++) {
            Thread t = new Thread(new MyConsumer<>(q, mutex));
            pool.submit(t);
        }
        pool.shutdown();
        while (!pool.isTerminated()) { } // wait
        System.out.println("Finished");
    }
}
