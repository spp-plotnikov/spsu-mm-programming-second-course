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
            T obj;
            mutex.lock();
            obj = target.poll();
            mutex.unlock();

            if (obj == null) {
                System.out.println("[Consumer] Can't consume - empty queue");
                return;
            }

            try {
                Thread.sleep(ThreadLocalRandom.current().nextLong(10, 20));
            } catch (InterruptedException e) {
                // none to be done :)
            }

            System.out.println("[Consumer] Thread " +
                    ((int) Thread.currentThread().getId() % mutex.n + 1) + " consumed object");
        }
        System.out.println("[Consumer] Thread " +
                ((int) Thread.currentThread().getId() % mutex.n + 1) + " has finished");
    }
}
