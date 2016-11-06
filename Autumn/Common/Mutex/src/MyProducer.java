import java.util.Queue;
import java.util.concurrent.ThreadLocalRandom;

public class MyProducer<T> implements Runnable {
    Queue<T> target;
    MyMutex mutex;
    T obj;

    public MyProducer(Queue<T> q, MyMutex m, T obj) {
        target = q;
        mutex = m;
        this.obj = obj;
    }

    public void run() {
        mutex.lock();
        try {
            Thread.sleep(ThreadLocalRandom.current().nextInt(50, 100));
            target.add(obj);
            System.out.println("Added object");
        }
        catch (InterruptedException e) {
            // nobody cares
        }
        finally {
            mutex.unlock();
        }
    }
}
