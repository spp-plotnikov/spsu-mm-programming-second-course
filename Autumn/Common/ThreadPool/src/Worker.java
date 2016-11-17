import java.util.Queue;

public class Worker extends Thread {
    private volatile Queue<Runnable> pending;

    public Worker(Queue<Runnable> q) {
        pending = q;
    }

    @Override
    public void run() {
        while (!isInterrupted()) {
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
                task.run();
            } catch (InterruptedException e) {
                System.out.println("interrupted");
                return;
            }
        }
    }
}
