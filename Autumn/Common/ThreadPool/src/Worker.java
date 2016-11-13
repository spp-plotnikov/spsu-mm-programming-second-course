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
                        pending.wait(50); // now it will not waste CPU
                    task = pending.poll();
                    if (task == null)
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
