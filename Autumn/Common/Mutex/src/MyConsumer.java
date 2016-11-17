import java.util.Queue;
import java.util.concurrent.ThreadLocalRandom;
import java.util.concurrent.atomic.AtomicBoolean;

public class MyConsumer<T> implements Runnable {
    Queue<T> target;
    MyMutex mutex;
    private volatile AtomicBoolean exitFlag;

    public MyConsumer(Queue<T> q, MyMutex m, AtomicBoolean exitFlag) {
        target = q;
        mutex = m;
        this.exitFlag = exitFlag;
    }

    public void run() {
        while (!exitFlag.get()) {
            T obj = null;
            try {
                mutex.lock();
                Thread.sleep(ThreadLocalRandom.current().nextInt(1, 7));
                obj = target.poll();
            } catch (InterruptedException e) {
                System.out.println("[Consumer] Thread " +
                        ((int) Thread.currentThread().getId() % mutex.n + 1) + " has been terminated");
            } finally {
                mutex.unlock();
            }

            if (obj == null) {
                System.out.println("[Consumer] Can't consume - empty queue");
                return;
            }
            System.out.println("[Consumer] Thread " +
                    ((int) Thread.currentThread().getId() % mutex.n + 1) + " consumed object");
        }
        System.out.println("[Consumer] Thread " +
                ((int) Thread.currentThread().getId() % mutex.n + 1) + " has finished");
    }
}
