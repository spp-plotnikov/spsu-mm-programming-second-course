import java.util.Queue;
import java.util.concurrent.ThreadLocalRandom;

/**
 * Created by milos on 11/4/16.
 */
public class MyConsumer<T> implements Runnable {
    Queue<T> target;
    MyMutex mutex;

    public MyConsumer(Queue<T> q, MyMutex m) {
        target = q;
        mutex = m;
    }

    public void run() {
        mutex.lock();
        try {
            Thread.sleep(ThreadLocalRandom.current().nextInt(50, 100));
            T obj = target.poll();
            if (obj == null) {
                System.out.println("Empty queue");
                return;
            }
            System.out.println("Consumed object");
        }
        catch (InterruptedException e) {
            /* no one cares */
        }
        finally {
            mutex.unlock();
        }
    }
}
