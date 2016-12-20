import java.io.IOException;
import java.util.LinkedList;
import java.util.Scanner;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Main {
    private static LinkedList<Integer> buffer = new LinkedList<>();
    private static final int producersCount = 2;
    private static final int consumersCount = 3;

    public static void main(String[] args) throws InterruptedException, IOException {
        Consumer[] consumers = new Consumer[consumersCount];
        Producer[] producers = new Producer[producersCount];

        int magicThreadPoolSize = producersCount + consumersCount;
        ExecutorService pool = Executors.newFixedThreadPool(magicThreadPoolSize);

        for (int i = 0; i < consumersCount; i++) {
            consumers[i] = new Consumer(buffer);
            pool.submit(consumers[i]);
        }

        for (int i = 0; i < producersCount; i++) {
            producers[i] = new Producer(buffer);
            pool.submit(producers[i]);
        }

        /*
         * Press Enter to stop. It's difficult to bind key listener to console, so sorry :(
        */

        Scanner s = new Scanner(System.in);
        s.nextLine();

        for (int i = 0; i < consumersCount; i++) {
            consumers[i].stop();
        }

        for (int i = 0; i < producersCount; i++) {
            producers[i].stop();
        }

        pool.shutdown();
    }
}

