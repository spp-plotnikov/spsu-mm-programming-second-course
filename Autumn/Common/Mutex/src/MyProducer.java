import java.util.Queue;
import java.util.concurrent.ThreadLocalRandom;
import java.util.concurrent.atomic.AtomicBoolean;

public class MyProducer<T> implements Runnable {
    Queue<T> target;
    MyMutex mutex;
    T obj;
    private volatile AtomicBoolean exitFlag;

    // sends obj to q syncing with m
    public MyProducer(Queue<T> q, MyMutex m, T obj, AtomicBoolean exitFlag) {
        this.exitFlag = exitFlag;
        target = q;
        mutex = m;
        this.obj = obj;
    }

    public void run() {
        while (!exitFlag.get()) {
            try {
                mutex.lock();
                Thread.sleep(ThreadLocalRandom.current().nextInt(50, 100));
                target.add(obj);
                System.out.println("[Producer] Thread " +
                        ((int) Thread.currentThread().getId() % mutex.n + 1) + " produced object");
            } catch (InterruptedException e) {
                System.out.println("[Producer] Thread " +
                        ((int) Thread.currentThread().getId() % mutex.n + 1) + " has been terminated");
                return;
            } finally {
                mutex.unlock();
            }
        }
    }
}
