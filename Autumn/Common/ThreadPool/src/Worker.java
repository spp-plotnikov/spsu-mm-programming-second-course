import java.util.Queue;
import java.util.concurrent.atomic.AtomicBoolean;

public class Worker extends Thread {
    private volatile Queue<Runnable> pending;
    private volatile AtomicBoolean isTerminated;

    public Worker(Queue<Runnable> q, AtomicBoolean flag) {
        pending = q;
        isTerminated = flag;
    }

    @Override
    public void run() {
        while (!isTerminated.get()) {
            try {
                Runnable task;
                synchronized (pending) {
                    while (pending.isEmpty())
                        pending.wait(50); // timeout=50, for just in case
                    task = pending.poll();
                    if (task == null) // null ptr, however the queue is NOT empty!
                        return;
                    pending.notifyAll();
                }
                task.run(); // and now we wait for it to finish anyway :(
            } catch (InterruptedException e) {
                System.out.println("never should happen");
                return;
            }
        }
    }
}
