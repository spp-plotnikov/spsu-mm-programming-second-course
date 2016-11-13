import java.util.Queue;
import java.util.LinkedList;

public class MyThreadPool {
    public static int threadMax = 2;
    private Queue<Runnable> pending;
    private Worker[] workers;

    public MyThreadPool() {
        pending = new LinkedList<>();
        workers = new Worker[threadMax];
    }

    public void enqueue(Runnable obj) {
        synchronized (pending) {
            pending.add(obj);
            pending.notify();
        }
    }

    public void start() {
        for (int i = 0; i < threadMax; i++) {
            Worker cur = new Worker(pending);
            cur.start();
            workers[i] = cur;
        }
    }

    public void stop() {
        System.out.println("terminating...");
        for (Worker cur : workers) {
            while (!pending.isEmpty()) pending.poll();
            try {
                cur.interrupt();
                synchronized (pending) {
                    pending.add(null); // empty task means "terminate"
                    pending.notifyAll();
                }
                cur.join();
            } catch (InterruptedException e) {
                System.out.println("this should not happen");
            }
        }
    }

    @Override
    protected void finalize() {
        this.stop();
    }
}
