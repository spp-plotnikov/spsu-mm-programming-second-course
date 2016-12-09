import java.util.LinkedList;

public class ThreadPool implements IDisposable{
    private final int POOL_SIZE = 4;
    private WorkerThread[] workers;
    private LinkedList<Runnable> taskQueue;
    private volatile Boolean running = true;

    public ThreadPool() {
        workers = new WorkerThread[POOL_SIZE];
        taskQueue = new LinkedList<>();

        for (int i = 0; i < POOL_SIZE; i++) {
            workers[i] = new WorkerThread();
            workers[i].start();
        }
    }

    public void enqueue(Runnable r) {
        synchronized (taskQueue) {
            taskQueue.add(r);
            taskQueue.notify();
        }
    }

    public void dispose() {
        running = false;
        synchronized (taskQueue) {
            taskQueue.clear();
            taskQueue.notifyAll();
        }

        for (WorkerThread worker : workers) {
            try {
                worker.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    private class WorkerThread extends Thread {
        @Override
        public void run() {
            Runnable r;

            while (running) {
                synchronized (taskQueue) {
                    while (taskQueue.isEmpty() && running) {
                        try {
                            taskQueue.wait();
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }

                    if (!running) {
                        break;
                    }

                    r = taskQueue.removeFirst();
                }

                try {
                    r.run();
                } catch (RuntimeException e) {
                    e.printStackTrace();
                }
            }
        }
    }
}
