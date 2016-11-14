public class ThreadPoolExecutor implements Runnable {

    private final TaskQueue taskQueue;
    private final Thread[] workers;

    public ThreadPoolExecutor(int numThreads) {
        taskQueue = new TaskQueue();
        workers = new Thread[numThreads];
        for (int i = 0; i < workers.length; i++) {
            workers[i] = new Thread(this);
            workers[i].start();
        }
    }

    public void add(Runnable r) {
        taskQueue.add(r);
        synchronized (taskQueue) {
            taskQueue.notify();
        }
    }

    public void shutdown() {
        for (int i = 0; i < workers.length; i++) {
            if(workers[i].isAlive()) {
                workers[i].interrupt();
            }
        }
    }

    public void run() {
        while(true) {
            Runnable r = taskQueue.pop();
            if (r == null) {
                synchronized(taskQueue) {
                    try {
                        while(taskQueue.isEmpty()) taskQueue.wait();
                    } catch (InterruptedException ex) {
                        break;
                    }
                }
            } else {
                r.run();
            }
        }
    }
}
