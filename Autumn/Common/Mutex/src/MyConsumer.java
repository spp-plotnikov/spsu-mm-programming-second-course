import java.util.Queue;
import java.util.concurrent.ThreadLocalRandom;

public class MyConsumer<T> implements Runnable {
    Queue<T> target;
    MyMutex mutex;

    public MyConsumer(Queue<T> q, MyMutex m) {
        target = q;
        mutex = m;
    }

    public void run() {
        while (true) {
            try {
                mutex.lock();
                Thread.sleep(ThreadLocalRandom.current().nextInt(50, 100));
                T obj = target.poll();
                if (obj == null) {
                    System.out.println("[Consumer] Can't consume - empty queue");
                    return;
                }
                System.out.println("[Consumer] Thread " +
                        ((int) Thread.currentThread().getId() % mutex.n + 1) + " consumed object");
            } catch (InterruptedException e) {
                System.out.println("[Consumer] Thread " +
                        ((int) Thread.currentThread().getId() % mutex.n + 1) + " has been terminated");
            } finally {
                mutex.unlock();
            }
        }
    }
}
